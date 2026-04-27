# MoCashRoCo
# MoCashRo Web Co. 

> A reusable, multi-tenant business website platform with integrated POS — built as a capstone project and designed to be a real, marketable product.

---

## What Is This?

MoCashRo Web Co. is a full-stack web application template that can be deployed, customized, and sold to small business clients across any industry. The idea: build it once, sell it many times.

Each deployment gives a business client:
- A polished, branded customer-facing storefront
- Online purchasing with a built-in Point of Sale (POS) system
- Live inventory management backed by a SQL database
- A secure admin portal to manage products, orders, and content
- A codebase they *own* — no Shopify subscriptions, no platform lock-in

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 9) |
| Language | C#, JavaScript, HTML5, CSS3 |
| Database | Microsoft SQL Server + Entity Framework Core |
| Frontend | Razor Views, Bootstrap 5 |
| Auth | ASP.NET Identity |
| IDE | Visual Studio 2022 |
| Version Control | Git / GitHub |

---

## Project Structure

```
MoCashRoWebCo/
├── Controllers/        # Route handling and business logic orchestration
├── Models/             # C# classes mapped to database tables
├── Views/              # Razor (.cshtml) pages rendered to the browser
├── wwwroot/            # Static assets: CSS, JS, images
├── docs/               # Project documentation
│   └── MoCashRoWebCo_SRS_RoughDraft.docx
└── README.md
```

---

## Core Features (Planned)

- [ ] Customer storefront with product catalog
- [ ] Shopping cart and checkout flow
- [ ] Simulated POS / order processing
- [ ] Inventory database with stock tracking
- [ ] Admin portal (CRUD for products, orders, content)
- [ ] Role-based authentication (Admin / Customer)
- [ ] Mobile-responsive UI
- [ ] Configurable branding per client deployment

---

## Documentation

The full Software Requirements Specification (SRS) lives in the `/docs` folder. It covers functional requirements, system architecture, database design, and the technically complex areas of the project including the MVC request lifecycle, controller-to-API relationships, and authentication architecture.

---

## About

**Student:** Molly Olson  
**Course:** Application Development Capstone  
**Instructor:** Professor Jesse Harlan  
**Version:** 0.1 — Rough Draft  

---

*MoCashRo Web Co. — misspelled on purpose. Built with intention.* ✨
