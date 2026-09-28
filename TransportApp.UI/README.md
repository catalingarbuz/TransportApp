# Prerequisites
You will need the latest LTS version of NodeJs (https://nodejs.org/en/) and a IDE of you choice, we recommend VSCode (https://code.visualstudio.com/) with plugins for Javascript, Typescript, CSS and SonarLint or WebStorm (https://www.jetbrains.com/webstorm/).

Also install the OpenApi generator after installing NodeJs via the command:

npm install @openapitools/openapi-generator-cli -g

# How to work with it
The project was started using Vite (https://vitejs.dev/guide/) for faster build times. The command for initializing the project was:

npm create vite@latest mobylab-web-app --template react-ts

To install the dependencies and start the UI, run these commands from the project root:

```bash
npm install
npm run start
```

Vite will print the local development URL in the terminal (usually http://localhost:5173). Press Ctrl+C to stop the server.

To generate the REST client for the project each time you modify it run the following command with the backend running:

openapi-generator-cli generate -i http://localhost:5000/swagger/v1/swagger.json -g typescript-fetch -o ./src/infrastructure/apis/client --additional-properties=supportsES6=true

To change environment variables change them in the .env file.

The default user/password to login with the backend is "admin@default.com" after the first migration is created and applied.