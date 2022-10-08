[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=TheRealLenon_KoIdentity&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=TheRealLenon_KoIdentity)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=TheRealLenon_KoIdentity&metric=coverage)](https://sonarcloud.io/summary/new_code?id=TheRealLenon_KoIdentity)

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=TheRealLenon_KoIdentity)](https://sonarcloud.io/summary/new_code?id=TheRealLenon_KoIdentity)
# KoIdentity


KoIdentity is a simple  and lightweight membership system primarily used for building ASP.NET Core web application, including membership, login and user data. It allows you to add authorization and authenticating features to your application and makes it easy to customize data on how to access specific parts of your application.

A simple user system allows to create, edit or delete a user. A role system allows individual roles to be created or deleted from the platform. At the same time, a previously created role can be assigned to a user, allowing specific endpoints to be limited to the respective role name through a simply held authorization.


## Run Locally

Before the project can be executed locally, the database connection must be adjusted.
This is possible either creating an ENV called **TekodingAzureDEVConnection** or changing
the connection string from the
[Program.cs](https://github.com/TheRealLenon/KoIdentity/blob/44b533bc62460b8b29c36d0a8ce0726501ac7bf4/examples/API/Program.cs#L25) directly.

Both ways the database needs to be migrated.

### 1. Clone the project

```zsh
  git clone https://github.com/TheRealLenon/KoIdentity
```

### 2. Go to the API directory

```zsh
  cd KoIdentity/examples/API
```

### 3. Build API

```zsh
  dotnet build
```

### 4. Start API

```zsh
  dotnet run
```


## Authors

- [@TheRealLenon](https://www.github.com/TheRealLenon)


## License

[MIT](https://choosealicense.com/licenses/mit/)

