# OnDemandTutorWebsite

A web platform for connecting students with tutors on demand.

## Table of Contents

- [About](#about)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Scripts](#scripts)
- [License](#license)
- [Author](#author)

## About

**OnDemandTutorWebsite** is designed to facilitate real-time connections between students and qualified tutors. The platform enables students to schedule sessions, communicate, and manage their learning experience efficiently.

## Features

- User authentication and authorization
- Real-time chat and notifications
- Scheduling and calendar integration
- Payment processing (integration ready)
- Dashboard with analytics and statistics
- Responsive, modern UI using Material-UI and Bootstrap
- Email notifications via EmailJS
- Tutor and student profile management

## Tech Stack

- **Frontend:** React, Material-UI, Bootstrap, SCSS
- **State Management:** React Context API, Formik, Yup
- **Charts & Scheduling:** ApexCharts, Syncfusion, React Widgets
- **Communication:** Microsoft SignalR, EmailJS
- **Testing:** Jest, React Testing Library
- **Build Tools:** React Scripts, Cross-env, Sass

## Getting Started

### Prerequisites

- Node.js (v18+ recommended)
- npm or yarn

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/tdtai09423/OnDemandTutorWebsite.git
   cd OnDemandTutorWebsite/FE/on-demand-tutor-ui
   ```

2. **Install dependencies:**

   ```bash
   npm install
   # or
   yarn install
   ```

3. **Start the development server:**

   ```bash
   npm start
   # or
   yarn start
   ```

   The UI will run locally, typically at `http://localhost:3000/`

### Building for Production

```bash
npm run build
# or
yarn build
```

## Project Structure

```
OnDemandTutorWebsite/
├── FE/
│   └── on-demand-tutor-ui/
│       ├── src/
│       ├── public/
│       ├── package.json
│       └── ...
└── ...
```

- `FE/on-demand-tutor-ui`: Frontend React application

## Scripts

- `start`: Start the development server
- `build`: Build the app for production
- `test`: Run tests
- `eject`: Eject configuration (not recommended unless necessary)
- `start:https`: Start with HTTPS and custom SSL certs

## License

This project is currently unlicensed. Please add a license for open source or private use as appropriate.

## Author

- [tdtai09423](https://github.com/tdtai09423)

---

> **Note:** Contributions, issues, and feature requests are welcome! Feel free to check the [issues page](https://github.com/tdtai09423/OnDemandTutorWebsite/issues).
