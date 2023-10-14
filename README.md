# Sale Markt e-Commerce Web Project 

![home](https://github.com/onrdr/SaleMarkt-Asp.NetCoreMVC/assets/106915107/d9dd36d3-c9f3-46a9-85b1-7b161d1fbc42)

This projects enables customers to 
  - List all the products
  - List all the categories
  - Register & Login
  - See the details of the product
  - Add products to Shopping List
  - Create Order for the shopping List
  - See the user details and edit
    
This projects enables admin to 
  - Create, List, Update and Delete products
  - Create, List, Update and Delete categories
  - Create, List, Update & Delete Users
  - Update Company Information
  - List all the orders and Confirm orders

# Project Structure
  ![event-app-structure-1](https://user-images.githubusercontent.com/106915107/228821415-7b3820ec-3d6c-4662-b60d-e63f8a6bb07e.png)
  
# Used Technologies and Tools:
Backend
- Monolith Architecture / NTier
- ASP.NET Core MVC
- ASP.NET Core Identity / Authentication & Authorization
- Entity Framework Core
- SQL Server
- AutoMapper
- Send Grid for Email Service
- Caching
- Localization
- Initial Database Seeding with Migration
- Docker
  
Frontend
- Razor Pages
- ViewComponents
- Partial Views
- Javascript
- JQuery
- HTML, CSS
- Bootstrap, BootsIcons
- Ajax Requests
- DataTables

# How to run the project in your local  
First of all, please make sure that the docker is running on your device.

### 1- Open a terminal or powershell and clone the repository
```
 git clone https://github.com/onrdr/SaleMarkt-Asp.NetCoreMVC
```

### 2- Navigate to the API Directory
```
 cd SaleMarkt-Asp.NetCoreMVC
```

### 3- Run the docker compose file
```
 docker compose up -d
```

### 4- Wait until container is initiated inside docker 
After completing sql image pull and project image created 
- What you will see in the powershell

![start-1](https://github.com/onrdr/SaleMarkt-Asp.NetCoreMVC/assets/106915107/ac7bfc0c-7cc3-4ac6-a4de-63cd781ed7d4)

- You will also see this in your docker desktop
 
![docker-wait](https://github.com/onrdr/SaleMarkt-Asp.NetCoreMVC/assets/106915107/c324649d-bc5c-4cc5-ab62-a0ab26340a10)

### 5- Wait until SQL Server connects and webui project starts. It will take approximately 1 minute 
- And then you will see healthy server and web app after a successfull setup

![healthy](https://github.com/onrdr/SaleMarkt-Asp.NetCoreMVC/assets/106915107/f4ca3493-d683-41be-9d60-146a31891521)

- You will also see this in your docker desktop
 
![docker-success](https://github.com/onrdr/SaleMarkt-Asp.NetCoreMVC/assets/106915107/0309e88b-f22d-4d04-b6d8-55ae33248e2c)

### 6- Open the aplication in any web browser and trust the certificate. Note that all the data was seeded. 
```
 https://localhost:8081
```

# Login Credentials for you to try the application
- SuperAdmin : 
  - username:
    ```
    superadmin@salemarkt.com
    ```
  - password:
    ```
    Super1*
    ```
- Company Admin : 
  - username:
    ```
    companyadmin@salemarkt.com
    ```
  - password:
    ```
    Admin1*
    ```
- Customer : 
  - username:
    ```
    customer@salemarkt.com
    ```
  - password:
    ```
    Customer1*
    ``` 

