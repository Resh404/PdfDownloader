# Storage Management System

The goal of this project is get familiar with design patterns, OOP, SOLID principles, and databases (MySQL),
where the focus is on the database interaction. 
A user should be able to interact with the storage management system, where the interaction performed 
by the user should be reflected in the database. The system needs to perform CRUD operations on the database.

## Description

The desired functionality have been achieved, where the core parts are the database interaction, and user terminal

The database contains information about the contents of storage system such as 
products, customers registered, and transaction history.
The database is queried by functions in order to create, read, update or delete the contents of the library.

The user terminal prompts a user with certain options the user can take and depending on what they choose different possibility opens op.
The database is updated after the user is done shopping in the warehouse and the changes can be seen in transaction_history table in the database.

## Getting Started

### Dependencies

* Windows 10
* PyCharm 2023.3.3
* MySQL server 8.3.0
* MySQL Workbench 8.0.36
* Python 3.9
* The necessary libraries can be found in the requirements.txt file

### Installing & Executing program

* Download the zip/clone from https://github.com/Resh404/storageManagementSystem
* Unzip at a desired location
* In pycharm go to "File -> Open" and choose the storageManagementSystem folder
* In the pycharm storageManagementSystem project terminal run:
```
pip install -r requirements.txt
```
* Next, in the project folder menu go the script:
"storageManagementDatabase -> database_connector.py" and edit it to fit your local MySQL server
* Now go to "main.py" script in the project folder menu and run "main.py".

## Authors & Help

[@Resh404](https://github.com/Resh404)
