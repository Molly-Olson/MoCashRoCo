const {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  HeadingLevel, AlignmentType, BorderStyle, WidthType, ShadingType,
  VerticalAlign, PageNumber, Header, Footer, LevelFormat, TableOfContents
} = require("docx");
const fs = require("fs");

// ── Helpers ──────────────────────────────────────────────────────────
const BORDER = { style: BorderStyle.SINGLE, size: 1, color: "CCCCCC" };
const BORDERS = { top: BORDER, bottom: BORDER, left: BORDER, right: BORDER };
const CELL_MARGINS = { top: 80, bottom: 80, left: 120, right: 120 };
const BRAND = "663399";
const ACCENT = "F59E0B";

function heading1(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_1,
    spacing: { before: 320, after: 160 },
    children: [new TextRun({ text, bold: true, size: 28, font: "Arial" })]
  });
}
function heading2(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_2,
    spacing: { before: 240, after: 120 },
    children: [new TextRun({ text, bold: true, size: 24, font: "Arial" })]
  });
}
function heading3(text) {
  return new Paragraph({
    spacing: { before: 200, after: 80 },
    children: [new TextRun({ text, bold: true, size: 22, font: "Arial", color: BRAND })]
  });
}
function para(text, opts = {}) {
  return new Paragraph({
    spacing: { after: 120 },
    children: [new TextRun({ text, size: 22, font: "Arial", ...opts })]
  });
}
function bullet(text, bold = false) {
  return new Paragraph({
    numbering: { reference: "bullets", level: 0 },
    spacing: { after: 60 },
    children: [new TextRun({ text, size: 22, font: "Arial", bold })]
  });
}
function cell(text, opts = {}) {
  const { width = 2340, shade = null, bold = false, color = null } = opts;
  return new TableCell({
    borders: BORDERS,
    width: { size: width, type: WidthType.DXA },
    margins: CELL_MARGINS,
    verticalAlign: VerticalAlign.CENTER,
    shading: shade ? { fill: shade, type: ShadingType.CLEAR } : undefined,
    children: [new Paragraph({
      children: [new TextRun({ text, size: 20, font: "Arial", bold, color: color || undefined })]
    })]
  });
}
function headerRow(texts, widths) {
  return new TableRow({
    tableHeader: true,
    children: texts.map((t, i) => new TableCell({
      borders: BORDERS,
      width: { size: widths[i], type: WidthType.DXA },
      margins: CELL_MARGINS,
      shading: { fill: BRAND, type: ShadingType.CLEAR },
      children: [new Paragraph({
        children: [new TextRun({ text: t, size: 20, font: "Arial", bold: true, color: "FFFFFF" })]
      })]
    }))
  });
}
function spacer() {
  return new Paragraph({ spacing: { after: 80 }, children: [] });
}

// ── Requirement tables ────────────────────────────────────────────────
const STATUS_COLORS = {
  "Complete": "D4EDDA",
  "In Progress": "FFF3CD",
  "Planned": "E2E3E5",
  "Stretch Goal": "F8D7DA"
};
const STATUS_TEXT_COLORS = {
  "Complete": "155724",
  "In Progress": "856404",
  "Planned": "383D41",
  "Stretch Goal": "721C24"
};

function reqTable(rows, colWidths) {
  return new Table({
    width: { size: 9360, type: WidthType.DXA },
    columnWidths: colWidths,
    rows: [
      headerRow(["ID", "Requirement", "Priority", "Status"], colWidths),
      ...rows.map(([id, req, pri, status]) => new TableRow({
        children: [
          cell(id, { width: colWidths[0], bold: true }),
          cell(req, { width: colWidths[1] }),
          cell(pri, { width: colWidths[2] }),
          cell(status, {
            width: colWidths[3],
            shade: STATUS_COLORS[status] || "E2E3E5",
            color: STATUS_TEXT_COLORS[status] || "000000",
            bold: status === "Complete"
          })
        ]
      }))
    ]
  });
}

function changelogTable(rows) {
  const widths = [1200, 1200, 1800, 5160];
  return new Table({
    width: { size: 9360, type: WidthType.DXA },
    columnWidths: widths,
    rows: [
      headerRow(["Version", "Date", "Author", "Changes"], widths),
      ...rows.map(([ver, date, auth, changes]) => new TableRow({
        children: [
          cell(ver, { width: widths[0], bold: true }),
          cell(date, { width: widths[1] }),
          cell(auth, { width: widths[2] }),
          cell(changes, { width: widths[3] })
        ]
      }))
    ]
  });
}

// ── Checklist table ───────────────────────────────────────────────────
function checklistTable(sections) {
  const widths = [600, 5760, 1500, 1500];
  const allRows = [headerRow(["#", "Feature", "Status", "Week"], widths)];
  let n = 1;
  for (const [sectionName, items] of sections) {
    // Section header row
    allRows.push(new TableRow({
      children: [new TableCell({
        columnSpan: 4,
        borders: BORDERS,
        margins: CELL_MARGINS,
        shading: { fill: "EDE6F5", type: ShadingType.CLEAR },
        children: [new Paragraph({
          children: [new TextRun({ text: sectionName, size: 20, font: "Arial", bold: true, color: BRAND })]
        })]
      })]
    }));
    for (const [feat, status, week] of items) {
      const done = status === "Complete";
      allRows.push(new TableRow({
        children: [
          cell(String(n++), { width: widths[0] }),
          cell((done ? "✓ " : "○ ") + feat, { width: widths[1], bold: done, color: done ? "155724" : "000000" }),
          cell(status, {
            width: widths[2],
            shade: STATUS_COLORS[status] || "E2E3E5",
            color: STATUS_TEXT_COLORS[status] || "000000",
            bold: done
          }),
          cell(week, { width: widths[3] })
        ]
      }));
    }
  }
  return new Table({ width: { size: 9360, type: WidthType.DXA }, columnWidths: widths, rows: allRows });
}

// ── Document ─────────────────────────────────────────────────────────
const doc = new Document({
  numbering: {
    config: [{
      reference: "bullets",
      levels: [{ level: 0, format: LevelFormat.BULLET, text: "•", alignment: AlignmentType.LEFT,
        style: { paragraph: { indent: { left: 720, hanging: 360 } } } }]
    }]
  },
  styles: {
    default: { document: { run: { font: "Arial", size: 22 } } },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 28, bold: true, font: "Arial", color: BRAND },
        paragraph: { spacing: { before: 320, after: 160 }, outlineLevel: 0 } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 24, bold: true, font: "Arial" },
        paragraph: { spacing: { before: 240, after: 120 }, outlineLevel: 1 } },
    ]
  },
  sections: [{
    properties: {
      page: {
        size: { width: 12240, height: 15840 },
        margin: { top: 1440, right: 1440, bottom: 1440, left: 1440 }
      }
    },
    headers: {
      default: new Header({ children: [
        new Paragraph({
          border: { bottom: { style: BorderStyle.SINGLE, size: 6, color: BRAND, space: 1 } },
          children: [
            new TextRun({ text: "MoCashRo Web Co. — Software Requirements Specification", size: 18, font: "Arial", color: "666666" }),
            new TextRun({ text: "\t", size: 18 }),
            new TextRun({ text: "v0.2 — May 2025", size: 18, font: "Arial", color: "666666" })
          ],
          tabStops: [{ type: "right", position: 9360 }]
        })
      ]})
    },
    footers: {
      default: new Footer({ children: [
        new Paragraph({
          border: { top: { style: BorderStyle.SINGLE, size: 6, color: "CCCCCC", space: 1 } },
          alignment: AlignmentType.CENTER,
          children: [
            new TextRun({ text: "Page ", size: 18, font: "Arial", color: "999999" }),
            new TextRun({ children: [PageNumber.CURRENT], size: 18, font: "Arial", color: "999999" }),
            new TextRun({ text: " of ", size: 18, font: "Arial", color: "999999" }),
            new TextRun({ children: [PageNumber.TOTAL_PAGES], size: 18, font: "Arial", color: "999999" })
          ]
        })
      ]})
    },
    children: [

      // ── Title Page ──────────────────────────────────────────────────
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { before: 720, after: 120 },
        children: [new TextRun({ text: "MoCashRo Web Co.", font: "Arial", size: 56, bold: true, color: BRAND })]
      }),
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { after: 80 },
        children: [new TextRun({ text: "Multi-Tenant Business Website Platform with Integrated POS", font: "Arial", size: 26, color: "444444" })]
      }),
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { after: 480 },
        children: [new TextRun({ text: "Software Requirements Specification", font: "Arial", size: 24, italics: true, color: "666666" })]
      }),

      // Metadata table
      new Table({
        width: { size: 5040, type: WidthType.DXA },
        columnWidths: [1800, 3240],
        rows: [
          ["Student", "Molly Olson"], ["Course", "Application Development Capstone"],
          ["Instructor", "Professor Jesse Harlan"], ["Original Date", "April 17, 2025"],
          ["Updated", "May 15, 2025"], ["Version", "0.2 — Updated Draft"]
        ].map(([k, v]) => new TableRow({ children: [
          new TableCell({ borders: BORDERS, width: { size: 1800, type: WidthType.DXA }, margins: CELL_MARGINS,
            shading: { fill: "EDE6F5", type: ShadingType.CLEAR },
            children: [new Paragraph({ children: [new TextRun({ text: k, size: 20, font: "Arial", bold: true })] })] }),
          new TableCell({ borders: BORDERS, width: { size: 3240, type: WidthType.DXA }, margins: CELL_MARGINS,
            children: [new Paragraph({ children: [new TextRun({ text: v, size: 20, font: "Arial" })] })] })
        ]}))
      }),

      spacer(), spacer(),

      // ── Changelog ──────────────────────────────────────────────────
      new Paragraph({ children: [new TextRun({ text: "", size: 22 })], pageBreakBefore: true }),
      heading1("Document Changelog"),
      para("This document is a living reference updated throughout the development lifecycle. All changes between versions are recorded below."),
      spacer(),

      changelogTable([
        ["0.1", "Apr 17, 2025", "Molly Olson", "Initial rough draft submission. All features listed as Planned. Full requirements, architecture, and complexity analysis written."],
        ["0.2", "May 15, 2025", "Molly Olson / Claude Sonnet 4.6", "Updated status fields for all completed features. Added Section 10 (Feature Completion Checklist). Added Section 11 (What Changed This Sprint). Updated Operating Environment to .NET 9. Noted session-based auth chosen over ASP.NET Identity for v1."],
      ]),

      spacer(), spacer(),

      // ── Feature Checklist ───────────────────────────────────────────
      heading1("Feature Completion Checklist"),
      para("This checklist tracks every feature against its implementation status. It is updated each sprint. Statuses: Complete (shipped and tested), In Progress (started, not done), Planned (not yet started), Stretch Goal (nice-to-have, post-MVP)."),
      spacer(),

      checklistTable([
        ["Storefront & Product Browsing", [
          ["Homepage with branding, hero section, featured products, category grid", "Complete", "Week 1-2"],
          ["Sticky navbar with cart badge, responsive mobile menu", "Complete", "Week 1-2"],
          ["Product catalog page with images, descriptions, prices from database", "Complete", "Week 1-2"],
          ["Product detail page with related products", "Complete", "Week 1-2"],
          ["Category filter on product listing", "Complete", "Week 1-2"],
          ["Keyword search on product listing", "Complete", "Week 1-2"],
          ["Sort products by name / price asc / price desc", "Complete", "Week 1-2"],
          ["About, Contact, Privacy pages", "Complete", "Week 1-2"],
          ["Custom brand theme (CSS variables for colors)", "Complete", "Week 2"],
          ["Mobile-responsive layout", "Complete", "Week 1-2"],
        ]],
        ["Shopping Cart", [
          ["Session-based cart (add, remove, update quantity, clear)", "Complete", "Week 1-2"],
          ["Cart count badge in navbar", "Complete", "Week 1-2"],
          ["Cart summary page with totals", "Complete", "Week 1-2"],
          ["Redirect back to product page after adding item", "Complete", "Week 1-2"],
          ["Checkout flow (contact info, review, simulated payment)", "Planned", "Week 3-4"],
          ["Order confirmation page", "Planned", "Week 3-4"],
          ["Inventory decrement on purchase", "Planned", "Week 3-4"],
          ["Prevent purchase of out-of-stock items", "Planned", "Week 3-4"],
        ]],
        ["Authentication & Accounts", [
          ["Customer registration (name, email, password)", "Complete", "Week 3"],
          ["Customer login / logout", "Complete", "Week 3"],
          ["Admin login (separate portal entry)", "Complete", "Week 3"],
          ["BCrypt password hashing", "Complete", "Week 3"],
          ["Session-based auth (UserId, UserName, UserRole in session)", "Complete", "Week 3"],
          ["Navbar shows logged-in user with sign-out dropdown", "Complete", "Week 3"],
          ["Default admin account seeded on first run", "Complete", "Week 3"],
          ["Route protection with [Authorize] on admin controllers", "Planned", "Week 3-4"],
          ["Redirect unauthenticated users from protected routes", "Planned", "Week 3-4"],
        ]],
        ["Admin Portal", [
          ["Admin dashboard with summary metrics", "Planned", "Week 4"],
          ["Product CRUD (create, edit, delete listings)", "Planned", "Week 4"],
          ["Inventory quantity management", "Planned", "Week 4"],
          ["Order list view (all orders, status, customer, total)", "Planned", "Week 4"],
          ["Order status update (Pending → Processing → Shipped → Complete)", "Planned", "Week 4"],
          ["Edit homepage content (business name, tagline, contact info)", "Planned", "Week 4"],
          ["Low-stock alerts on dashboard", "Stretch Goal", "Post-MVP"],
          ["Multiple admin account management", "Stretch Goal", "Post-MVP"],
        ]],
        ["Database & Architecture", [
          ["SQL Server LocalDB with EF Core Code-First", "Complete", "Week 1"],
          ["Migrations applied (InitialCreate)", "Complete", "Week 1"],
          ["All core models: Product, Category, Order, OrderItem, Customer, Admin, SiteSettings, CartItem", "Complete", "Week 1"],
          ["Seeded data: 5 categories, 5 products, SiteSettings", "Complete", "Week 1"],
          ["Service layer (thin controllers)", "Planned", "Week 4"],
          ["Order record written to DB on checkout", "Planned", "Week 4"],
        ]],
        ["Future / Stretch Goals", [
          ["Live payment gateway (Stripe or Square)", "Stretch Goal", "Post-MVP"],
          ["Email order confirmations", "Stretch Goal", "Post-MVP"],
          ["Admin-controlled theming (color, logo, business name via portal)", "Stretch Goal", "Post-MVP"],
          ["Sales analytics dashboard with charts", "Stretch Goal", "Post-MVP"],
          ["Multi-instance deployment tooling for client onboarding", "Stretch Goal", "Post-MVP"],
          ["Subscription/maintenance tier billing system", "Stretch Goal", "Post-MVP"],
        ]],
      ]),

      spacer(), spacer(),

      // ── What Changed This Sprint ─────────────────────────────────────
      heading1("What Changed This Sprint (Week 3 — May 2025)"),
      para("This section documents the concrete work completed since the original SRS submission on April 17, 2025."),
      spacer(),

      heading2("Completed Features"),
      bullet("Session-based authentication system built from scratch", true),
      bullet("AccountController with Login, Register, AdminLogin, and Logout actions"),
      bullet("Customer registration with BCrypt password hashing (BCrypt.Net-Next v4.2.0)"),
      bullet("Customer and admin login with session storage (UserId, UserName, UserRole)"),
      bullet("Three new Razor views: Login, Register, AdminLogin — fully styled with brand theme"),
      bullet("Navbar dynamically shows logged-in user name with sign-out dropdown when authenticated"),
      bullet("Default admin account (username: admin) seeded automatically on first application run"),
      spacer(),

      heading2("Architecture Decisions Made"),
      bullet("Session-based auth chosen over ASP.NET Identity — rationale: simpler configuration for a capstone scope, existing Admin/Customer models already in the DB, avoids identity scaffolding complexity. Can migrate to Identity in a future version."),
      bullet("BCrypt.Net-Next selected for password hashing — industry standard, well-maintained, straightforward API"),
      bullet(".NET target updated from .NET 8 (planned) to .NET 9 (current LTS used in implementation)"),
      spacer(),

      heading2("Technical Debt / Notes"),
      bullet("Route protection ([Authorize] attribute or custom middleware) not yet applied to admin routes — this is the immediate next task"),
      bullet("Admin portal controllers and views do not yet exist — scaffolding is the Week 4 priority"),
      bullet("Checkout flow is the largest remaining feature gap before the app qualifies as a functional e-commerce store"),
      spacer(), spacer(),

      // ── Introduction (unchanged) ─────────────────────────────────────
      new Paragraph({ children: [new TextRun({ text: "" })], pageBreakBefore: true }),
      heading1("1. Introduction"),
      heading2("1.1 Purpose"),
      para("This Software Requirements Specification (SRS) defines the functional and non-functional requirements for MoCashRo Web Co., a reusable, multi-tenant web application platform built as a capstone project. The purpose of this document is to serve as a living reference throughout the development lifecycle and will evolve as requirements are refined."),
      para("This document is intended for the instructor evaluating the capstone project and will guide the developer (the student) in making architectural and implementation decisions."),

      heading2("1.2 Project Overview"),
      heading3("Problem Statement"),
      para("Many small and local businesses lack affordable, professionally built websites with integrated e-commerce capabilities. Off-the-shelf solutions like Shopify exist but require ongoing subscription fees and offer limited customization. MoCashRo Web Co. aims to be a developer-owned, resellable platform: a single codebase that can be deployed, customized, and sold or licensed to individual business clients across different industries."),
      para("The original inspiration was a neighbor's landscaping business that needed a website. This evolved into a broader vision: a marketable, reusable template with a built-in Point-of-Sale (POS) system, inventory management, and an admin portal, all architected to be adaptable to the needs of any small business client."),

      heading2("1.3 Document Scope"),
      para("This SRS covers the following major system components:"),
      bullet("Customer-facing storefront with product browsing and online purchasing"),
      bullet("Integrated POS / shopping cart and checkout system"),
      bullet("Inventory and product database (SQL)"),
      bullet("Admin portal for content, inventory, and order management"),
      bullet("Authentication system for admin and customer roles"),
      bullet("Reusable MVC architecture (ASP.NET Core) for multi-client deployment"),

      spacer(),
      heading2("1.4 Definitions and Acronyms"),
      new Table({
        width: { size: 9360, type: WidthType.DXA },
        columnWidths: [2000, 7360],
        rows: [
          headerRow(["Term", "Definition"], [2000, 7360]),
          ...([
            ["MVC", "Model-View-Controller: an architectural pattern separating data (Model), UI (View), and logic (Controller)"],
            ["POS", "Point of Sale: the system through which customers complete purchases"],
            ["SRS", "Software Requirements Specification: this document"],
            ["ASP.NET Core", "Microsoft's open-source web framework for building web apps in C#"],
            ["SQL", "Structured Query Language: used to manage relational database data"],
            ["Admin Portal", "A secure, login-gated interface for business owners to manage their site"],
            ["Tenant", "A single business client using a deployed instance of MoCashRo Web Co."],
            ["CRUD", "Create, Read, Update, Delete: the four basic database operations"],
            ["BCrypt", "Industry-standard password hashing algorithm — used in this project via BCrypt.Net-Next"],
          ]).map(([t, d]) => new TableRow({ children: [
            cell(t, { width: 2000, bold: true, shade: "F5F0FA" }),
            cell(d, { width: 7360 })
          ]}))
        ]
      }),

      spacer(), spacer(),

      // ── System Description ───────────────────────────────────────────
      new Paragraph({ children: [new TextRun({ text: "" })], pageBreakBefore: true }),
      heading1("2. Overall System Description"),
      heading2("2.1 System Perspective"),
      para("MoCashRo Web Co. is a self-contained web application. Each deployed instance represents a single business tenant. The system consists of a customer-facing front end, an admin portal, a business logic layer (controllers/services), and a SQL database. The architecture follows ASP.NET Core MVC conventions, with Razor Views for HTML rendering, C# controllers for routing and logic, and Entity Framework Core for database access."),

      heading2("2.2 User Classes"),
      bullet("Guest / Anonymous Customer — Browses products, adds items to cart, completes checkout without an account (or optionally creates one)."),
      bullet("Registered Customer — Has a saved account, order history, and profile information."),
      bullet("Admin User — Business owner or staff member. Has full access to the admin portal: inventory, orders, content, and settings."),
      bullet("Super Admin (Developer) — The developer. Has access to configuration, theming, and deployment settings for the template itself."),

      heading2("2.3 Operating Environment"),
      bullet("IDE: Visual Studio 2022"),
      bullet("Framework: ASP.NET Core MVC (.NET 9)"),
      bullet("Languages: C#, JavaScript, HTML5, CSS3"),
      bullet("Database: Microsoft SQL Server LocalDB (dev) / Azure SQL (optional cloud deployment)"),
      bullet("Front End: Bootstrap 5, Bootstrap Icons CDN, jQuery"),
      bullet("Version Control: Git / GitHub (github.com/Molly-Olson/MoCashRoCo)"),

      heading2("2.4 Design and Implementation Constraints"),
      bullet("Must be deployable as a standalone website for a single business tenant per instance."),
      bullet("Must follow MVC architecture to maintain clear separation of concerns."),
      bullet("Admin portal must be secured behind authenticated login."),
      bullet("All product and order data must persist in a relational SQL database."),
      bullet("Front-end must be visually polished and responsive (mobile-friendly)."),
      bullet("Codebase must be clean and maintainable to enable reuse across client deployments."),

      heading2("2.5 Assumptions and Dependencies"),
      bullet("Each client deployment requires a separate instance of the application and database."),
      bullet("Payment processing will initially be simulated (no live payment gateway required for capstone)."),
      bullet("No multi-tenancy at the database level is required for v1.0."),

      spacer(), spacer(),

      // ── Functional Requirements ──────────────────────────────────────
      new Paragraph({ children: [new TextRun({ text: "" })], pageBreakBefore: true }),
      heading1("3. Functional Requirements"),
      heading2("3.1 Customer Storefront"),
      reqTable([
        ["FR-101", "The system shall display a homepage with business branding, featured products, and navigation.", "High", "Complete"],
        ["FR-102", "The system shall display a product catalog with images, descriptions, and prices pulled from the database.", "High", "Complete"],
        ["FR-103", "The system shall allow customers to filter or search products by category.", "Medium", "Complete"],
        ["FR-104", "The system shall allow customers to add products to a shopping cart.", "High", "Complete"],
        ["FR-105", "The system shall display a cart summary with item quantities and totals.", "High", "Complete"],
        ["FR-106", "The system shall allow customers to proceed through a checkout flow (contact info, order review, simulated payment).", "High", "Planned"],
        ["FR-107", "The system shall display an order confirmation page upon successful checkout.", "High", "Planned"],
      ], [800, 5560, 1200, 1800]),

      spacer(),
      heading2("3.2 POS and Order Management"),
      reqTable([
        ["FR-201", "The system shall create an order record in the database upon checkout completion.", "High", "Planned"],
        ["FR-202", "The system shall decrement inventory quantities when an order is placed.", "High", "Planned"],
        ["FR-203", "The system shall prevent purchase of items with zero inventory.", "High", "Planned"],
        ["FR-204", "The admin portal shall display a list of all orders with status, customer info, and totals.", "High", "Planned"],
        ["FR-205", "The admin portal shall allow updating the status of an order (Pending, Processing, Shipped, Complete).", "Medium", "Planned"],
      ], [800, 5560, 1200, 1800]),

      spacer(),
      heading2("3.3 Admin Portal"),
      reqTable([
        ["FR-301", "The system shall restrict the admin portal to authenticated users only.", "High", "Planned"],
        ["FR-302", "The admin portal shall allow creating, editing, and deleting product listings.", "High", "Planned"],
        ["FR-303", "The admin portal shall allow managing inventory quantities per product.", "High", "Planned"],
        ["FR-304", "The admin portal shall allow editing homepage content (business name, tagline, contact info).", "Medium", "Planned"],
        ["FR-305", "The admin portal shall display a dashboard with summary metrics (total orders, low stock alerts).", "Low", "Stretch Goal"],
        ["FR-306", "The system shall support creating and managing multiple admin accounts.", "Low", "Stretch Goal"],
      ], [800, 5560, 1200, 1800]),

      spacer(),
      heading2("3.4 Authentication and Authorization"),
      reqTable([
        ["FR-401", "The system shall provide a login page for admin users.", "High", "Complete"],
        ["FR-402", "The system shall hash and securely store user passwords.", "High", "Complete"],
        ["FR-403", "The system shall redirect unauthenticated users away from protected admin routes.", "High", "Planned"],
        ["FR-404", "The system shall support session-based authentication.", "High", "Complete"],
        ["FR-405", "Customer account registration and login shall be supported.", "Medium", "Complete"],
      ], [800, 5560, 1200, 1800]),

      spacer(), spacer(),

      // ── Non-Functional Requirements ──────────────────────────────────
      new Paragraph({ children: [new TextRun({ text: "" })], pageBreakBefore: true }),
      heading1("4. Non-Functional Requirements"),
      heading2("4.1 Usability"),
      bullet("The customer-facing storefront shall be visually appealing and consistent with the client business's branding."),
      bullet("Navigation shall be intuitive; a first-time visitor should locate and purchase a product within 3 clicks."),
      bullet("The admin portal shall be clearly labeled and require no training for basic CRUD functions."),
      bullet("The UI shall be responsive and functional on mobile, tablet, and desktop screen sizes."),

      heading2("4.2 Performance"),
      bullet("Product listing pages shall load within 3 seconds on a standard broadband connection."),
      bullet("Cart and checkout operations shall complete within 2 seconds under normal load."),

      heading2("4.3 Maintainability and Reusability"),
      bullet("Business-specific configuration (name, colors, logo, categories) shall be stored in a SiteSettings database row to minimize code changes between client deployments."),
      bullet("Controllers shall remain thin; business logic shall be encapsulated in separate service classes."),

      heading2("4.4 Security"),
      bullet("All admin routes shall be protected via authentication attributes ([Authorize] or equivalent session check)."),
      bullet("User input shall be validated server-side to prevent SQL injection and XSS attacks."),
      bullet("Passwords shall be stored using BCrypt hashing (implemented via BCrypt.Net-Next)."),

      spacer(), spacer(),

      // ── Architecture ─────────────────────────────────────────────────
      new Paragraph({ children: [new TextRun({ text: "" })], pageBreakBefore: true }),
      heading1("5. System Architecture Overview"),
      heading2("5.1 High-Level Architecture"),
      new Table({
        width: { size: 9360, type: WidthType.DXA },
        columnWidths: [2400, 6960],
        rows: [
          headerRow(["Layer", "Technology / Responsibility"], [2400, 6960]),
          ...([
            ["Presentation", "Razor Views (.cshtml), HTML5, CSS3 (Bootstrap 5, Bootstrap Icons), jQuery"],
            ["Routing / Control", "ASP.NET Core MVC Controllers (C#) — HomeController, ProductsController, CartController, AccountController"],
            ["Business Logic", "C# Service Classes (planned: OrderService, CartService)"],
            ["Data Access", "Entity Framework Core 9 (Code-First) with AppDbContext"],
            ["Database", "SQL Server LocalDB: Products, Categories, Orders, OrderItems, Customers, Admins, SiteSettings"],
            ["Authentication", "Custom session-based auth with BCrypt password hashing (v1); upgrade to ASP.NET Identity planned"],
          ]).map(([l, t]) => new TableRow({ children: [
            cell(l, { width: 2400, bold: true, shade: "F5F0FA" }),
            cell(t, { width: 6960 })
          ]}))
        ]
      }),

      spacer(), spacer(),

      // ── Business Viability ────────────────────────────────────────────
      heading1("6. Business Viability and Future Scope"),
      heading2("6.1 Viability Analysis"),
      para("MoCashRo Web Co. targets a different value proposition than SaaS platforms like Shopify or Square:"),
      bullet("Full customization that SaaS platforms cannot offer — unique UI, custom workflows, no recurring platform fees for the client."),
      bullet("Ownership — the business owns their codebase and data, not a third-party SaaS company."),
      bullet("Developer as a service — a one-time build fee plus monthly retainer for hosting, updates, backups, and support is a proven agency model."),

      heading2("6.2 Planned Future Enhancements"),
      bullet("Subscription / maintenance tier system for recurring client revenue"),
      bullet("Live payment gateway integration (Stripe or Square API)"),
      bullet("Email notification system for order confirmations"),
      bullet("Admin dashboard with sales analytics and charts"),
      bullet("Configurable theming system (colors, font, logo) via admin portal"),
      bullet("Multi-instance deployment tooling for streamlined client onboarding"),

      spacer(), spacer(),

      // ── Version History ──────────────────────────────────────────────
      new Paragraph({ children: [new TextRun({ text: "" })], pageBreakBefore: true }),
      heading1("9. Document Version History"),
      changelogTable([
        ["0.1", "Apr 17, 2025", "Molly Olson", "Initial rough draft. All features Planned. Full requirements, architecture, complexity analysis."],
        ["0.2", "May 15, 2025", "Molly Olson", "Updated status fields for completed features. Added Changelog, Feature Checklist, and Sprint Summary sections. Auth system documented. .NET 9 noted."],
      ]),
      spacer(),
      para("This is a living document. It will be updated throughout the capstone project as requirements are refined, features are added or descoped, and implementation details become clearer.", { italics: true, color: "666666" }),
    ]
  }]
});

Packer.toBuffer(doc).then(buffer => {
  const outPath = "C:/Users/olson/source/repos/Capstone/MoCashRoCo/MoCashRo Web Co._SRS_v0.2.docx";
  fs.writeFileSync(outPath, buffer);
  console.log("Written: " + outPath);
});
