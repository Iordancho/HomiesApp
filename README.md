# HomiesApp

## Overview
HomiesApp is a community-based application that allows users to connect, share resources, and collaborate on projects or activities within their neighborhood or group. It fosters local interaction by enabling users to create and manage group activities or resource-sharing events.

## Key Features

### For Users
- **Group Creation**: Create groups for specific communities or interests, allowing users to join and participate.
- **Event Management**: Organize, schedule, and manage events or activities for the community.
- **Resource Sharing**: Facilitate sharing of resources such as tools, books, or other items between community members.
- **Messaging**: Built-in messaging system to allow communication within groups.
- **Notifications**: Get notified about upcoming events or new messages within your groups.

### For Administrators
- **User Management**: Manage user access, group memberships, and permissions.
- **Event Moderation**: Approve or moderate events and activities posted by group members.
- **Resource Oversight**: Monitor the sharing of resources to ensure fairness and availability.

## Technical Details
This application is built using the following technologies:
- **ASP.NET Core**: Provides the back-end framework for handling HTTP requests, user authentication, and business logic.
- **Razor Pages**: Enables dynamic generation of HTML views and facilitates seamless interaction between the front-end and back-end.
- **Entity Framework Core**: Used to interact with the database, handling all data storage, retrieval, and manipulation.
- **SQL Server**: The application uses SQL Server as the relational database for storing users, groups, events, and resources.
- **Identity Framework**: Ensures secure user authentication, role management, and authorization.
- **SignalR**: Implements real-time messaging between users for the in-app chat feature.
- **Bootstrap**: Front-end framework used to create a responsive and user-friendly interface.
- **REST API**: Exposes endpoints for managing user data, groups, events, and other related operations through RESTful services.
