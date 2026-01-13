# Wedding Chatbot – Web Application Overview

## Project Introduction

**Wedding Chatbot** is a web-based Wedding Guest Invitation and RSVP Management System that automates guest communication through **WhatsApp**.  
The system allows wedding hosts to invite guests, collect RSVP responses, and track attendance efficiently using a chatbot-driven workflow.

This project was developed as a **Final Year Project (PSM)** and follows a **web-based architecture** using **ASP.NET (Razor Pages / MVC)** for the backend and **WhatsApp API integration** for real-time messaging.

---

## System Objectives

The main objectives of this system are:

- Automate wedding invitations via WhatsApp
- Collect RSVP responses without manual follow-up
- Store and summarize guest responses in real time
- Reduce human error in guest list management
- Provide a centralized web portal for wedding hosts

---

## System Architecture Overview

The system consists of **three main components**:

1. **Web Portal**
   - Used by wedding hosts (admin/users)
   - Manage events, guest lists, and chatbot questions
   - View RSVP summaries and guest responses

2. **WhatsApp Chatbot**
   - Sends invitation messages automatically
   - Interacts with guests using predefined questions
   - Validates guest replies and requests re-input when invalid

3. **Database**
   - Stores users, events, guests, chatbot questions, and responses
   - Enables reporting and RSVP tracking

---

## 🛠️ Technology Stack

| Layer | Technology |
|-----|-----------|
| Backend | ASP.NET Core (Razor / MVC) |
| Frontend | HTML, CSS, JavaScript, jQuery |
| Database | MySQL |
| Messaging | WhatsApp API |
| Architecture | MVC Pattern |
| Version Control | Git & GitHub |

---

## 📂 Code Structure Overview

The project follows a **standard ASP.NET MVC / Razor structure** to ensure scalability and maintainability.

### 🔹 Controllers
Handles HTTP requests and application logic.
