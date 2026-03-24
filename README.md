# MicroServices Architecture

## Overview

**Distributed microservices architecture** built with **.NET 8 (C#)** for an e-commerce platform with multiple independent services communicating via REST, gRPC, and event-driven messaging.

**Key Features:**
- Multi-service deployment with independent databases
- API Gateway (Ocelot) for unified request routing
- Event-driven communication via RabbitMQ
- JWT-based authentication across services
- gRPC for high-performance internal calls

---

## Architecture Diagram

![Diagram](diagram.png)


---

## Microservices

| Service | Port | Database | Purpose |
|---------|------|----------|---------|
| **Authentication.API** | 5001-5003 | PostgreSQL | User registration, JWT tokens, login |
| **Catalog.API** | 5004-5006 | MongoDB | Product management, calls Discount.GRPC |
| **Basket.API** | 5007-5008 | Redis | Shopping cart, publishes CheckoutEvent |
| **Discount.API** | 5009 | PostgreSQL | REST API for discounts |
| **Discount.GRPC** | 5010-5011 | PostgreSQL | gRPC service for fast discount lookups |
| **Ordering.API** | 5012-5013 | MongoDB | Order management, consumes CheckoutEvent |
| **User.API** | 5014-5015 | PostgreSQL | User profiles, consumes RegistrationEvent |

---

## Communication Patterns

### 1. **Synchronous (REST via Gateway)**
- **Client → Ocelot Gateway → Services**
- All external requests go through API Gateway
- Routes: `/auth/*`, `/catalog/*`, `/basket/*`, `/discount/*`, `/orders/*`, `/user/*`

### 2. **Synchronous (gRPC - Internal)**
- **Catalog.API → Discount.GRPC** (for fast discount retrieval)
- Binary Protocol Buffer serialization (~10x faster than JSON)
- Used only for inter-service communication

### 3. **Asynchronous (Event Bus)**
- **RabbitMQ + MassTransit** for decoupled services

**Events:**
1. **BasketCheckoutEvent** - Published by Basket.API → Consumed by Ordering.API
   - Triggers order creation when user checks out
   
2. **UserRegistrationEvent** - Published by Auth.API → Consumed by User.API
   - Syncs user profile data across services

---

## Database Strategy

**Database per Service Pattern** - Each service owns its data:

| Service | Database | Why |
|---------|----------|-----|
| Authentication, Discount, User | **PostgreSQL** | Relational data, ACID transactions, complex queries |
| Catalog, Ordering | **MongoDB** | Document flexibility, High throughput, Schema evolution |
| Basket | **Redis** | Fast session/state caching, Temporary data |

---

## Authentication & Security

**JWT-Based Flow:**
1. User logs in → Auth.API validates credentials
2. Auth.API generates JWT token (signed with secret key)
3. Client includes token in all requests: `Authorization: Bearer <token>`
4. Ocelot validates token signature and expiration
5. Services verify token claims (user ID, role)

**Configuration (appsettings.json):**
```json
{
  "AppSettings": {
    "Token": "your-secret-key-minimum-32-characters",
    "Issuer": "MicroServicesAuth",
    "Audience": "MicroServicesClients",
    "ExpirationMinutes": 1440
  }
}
```

All services validate JWT:
- Signature verification ✓
- Expiration check ✓
- Issuer & Audience validation ✓

---

## Event-Driven Communication (Async)

### **User Registration Flow:**
```
1. User registers → Auth.API
2. Auth.API creates user in PostgreSQL
3. Auth.API publishes UserRegistrationEvent to RabbitMQ
4. User.API consumer receives event
5. User.API creates profile in PostgreSQL
   (User is NOW eventually consistent between services)
```

### **Checkout Flow:**
```
1. Customer clicks checkout → Basket.API
2. Basket.API validates & publishes BasketCheckoutEvent
3. RabbitMQ routes to BasketCheckoutQueue
4. Ordering.API consumer picks up event
5. Ordering.API creates Order in MongoDB
   (Order immediately visible to user)
```

### **MassTransit Configuration:**
```csharp
// Publisher setup
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

// Consumer setup
config.AddConsumer<CheckoutEventConsumer>();
config.ReceiveEndpoint("BasketCheckoutQueue", e =>
{
    e.ConfigureConsumer<CheckoutEventConsumer>(context);
});
```

**Event Base Class:**
```csharp
public abstract class IntegrationBaseEvent
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
}
```

---

## Technology Stack

**Backend:**
- ASP.NET Core 8
- Entity Framework Core (PostgreSQL)
- MongoDB C# Driver
- StackExchange.Redis
- MassTransit (RabbitMQ abstraction)
- Ocelot (API Gateway)
- gRPC & Protocol Buffers
- JWT Bearer Authentication

**Frontend:**
- React 18 + Vite
- TypeScript
- Tailwind CSS (Admin)

**Infrastructure:**
- PostgreSQL 17 (relational data)
- MongoDB 7 (document storage)
- Redis Alpine (caching)
- RabbitMQ 3-management (messaging)
- Docker & Docker Compose

---

## API Gateway (Ocelot) Routing

**Routes configuration:**
```
/auth/*      → Authentication.API:5001
/catalog/*   → Catalog.API:5004
/basket/*    → Basket.API:5007
/discount/*  → Discount.API:5009
/orders/*    → Ordering.API:5012
/user/*      → User.API:5014
```

**CORS Configuration:**
- Allowed origins: `localhost:5173, localhost:5174, localhost:3005, localhost:3006`
- Allows credentials, any headers, any method

**Request Flow:**
1. Frontend → POST `/auth/login` to Gateway
2. Gateway validates JWT (if exists)
3. Gateway matches route: `/auth/*` → Auth service
4. Gateway forwards request with proper headers
5. Service processes, returns response
6. Gateway applies middleware, sends to client

---

## Deployment Architecture

**Docker Compose Services:**

**Databases:**
- authdb (PostgreSQL)
- discountdb (PostgreSQL)
- userdb (PostgreSQL)
- catalogdb (MongoDB)
- orderdb (MongoDB)
- basketdb (Redis)

**Message Bus:**
- rabbitmq (RabbitMQ)

**Microservices:**
- authentication.api
- catalog.api
- basket.api
- discount.api
- discount.grpc
- ordering.api
- user.api

**API Gateway:**
- ocelotapigw

**Frontend:**
- frontend-admin (Nginx-served React)
- frontend-customer (Nginx-served React)

**Connection Strings:**
```
PostgreSQL:  Host={service-name};Port=5432;Database={db};Username=postgres;Password=password
MongoDB:     mongodb://{service-name}:27017
Redis:       {service-name}:6379
RabbitMQ:    amqp://guest:guest@rabbitmq:5672
```

**Startup Sequence:**
1. Databases (PostgreSQL, MongoDB, Redis)
2. RabbitMQ
3. Authentication.API (seed data)
4. Other Services (Catalog, Basket, Discount, Ordering, User)
5. Ocelot Gateway
6. Frontend Applications

---

## Service Dependencies

**Direct Service Calls:**
- Catalog.API → Discount.GRPC (gRPC, synchronous)

**Event Subscriptions:**
- Ordering.API ← Basket.API (BasketCheckoutEvent)
- User.API ← Auth.API (UserRegistrationEvent)

**Shared Dependencies:**
- All services: MassTransit, JWT Bearer, Swagger
- All depend on: RabbitMQ (event bus)

---

## Key Design Patterns

| Pattern | Implementation |
|---------|-----------------|
| **Microservices** | 7 independent deployable services |
| **Database per Service** | Each service owns data |
| **API Gateway** | Ocelot for unified entry point |
| **Event Sourcing** | RabbitMQ pub/sub for async communication |
| **gRPC** | High-performance internal service calls |
| **JWT Authentication** | Stateless token-based security |
| **Health Checks** | `/health` endpoint on each service |
| **Docker Containerization** | Consistent development & production |

---

## Monitoring & Health

Each service exposes `/health` endpoint:
```
GET /health
Response: { "status": "Healthy", "checks": {...} }
```

Services monitor:
- Database connectivity
- RabbitMQ availability
- External service dependencies

---

## Scaling Considerations

**Horizontal Scaling:** All services are stateless and can be scaled independently
**Load Balancing:** Route multiple instances of same service
**Database Scaling:** PostgreSQL & MongoDB configured for replication
**Cache Layer:** Redis shared across Basket instances
**Event Bus:** RabbitMQ handles high throughput messaging

---

## Frontend Screenshots

### Admin Dashboard (Frontend-Admin)

**Dashboard Home**
![Admin Dashboard](src/Frontend-Admin/assests/Screenshot138.png)

**Books Management**
![Books Management](src/Frontend-Admin/assests/Screenshot139.png)

**Discounts Management**
![Discounts Management](src/Frontend-Admin/assests/Screenshot140.png)

**Orders Management**
![Orders Management](src/Frontend-Admin/assests/Screenshot141.png)

**Admin Profile**
![Admin Profile](src/Frontend-Admin/assests/Screenshot143.png)

**Admin Login**
![Admin Login](src/Frontend-Admin/assests/Screenshot144.png)

### Customer Store (Frontend-Customer)

**Books Catalog**
![Books Catalog](src/Frontend-Customer/assets/Screenshot132.png)

**Shopping Cart**
![Shopping Cart](src/Frontend-Customer/assets/Screenshot133.png)

**Customer Orders**
![Customer Orders](src/Frontend-Customer/assets/Screenshot134.png)

**Customer Profile**
![Customer Profile](src/Frontend-Customer/assets/Screenshot135.png)

**Login Page**
![Login Page](src/Frontend-Customer/assets/Screenshot136.png)

**Register Page**
![Register Page](src/Frontend-Customer/assets/Screenshot137.png)




##  How to Run

### Prerequisites
- Docker  

---

### 1. Clone the Repository
```bash
git clone https://github.com/Abdellah-saim-mamoune1/MicroServices.git
cd MicroServices
docker-compose up -d rabbitmq
docker-compose up -d --build
