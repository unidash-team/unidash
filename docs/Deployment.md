# Deployment
*For an architectural overview check out [this page](Architecture.md).*

## Requirements
- At least 4 GB RAM
- A host that is accessible from the outside
- A running Docker instance with [Docker Swarm fully set up](https://docs.docker.com/engine/reference/commandline/swarm_init/)

## Deploy with Docker Stack
The first step is to customize your Docker compose file. Download these files and adjust them to fit your needs.
- Base file: [docker-compose.yml](../src/docker-compose.yml)
- Treafik file: [docker-compos.traefik.yml](../src/docker-compose.traefik.yml)
- Environment file: [docker-compose.dev.yml](../src/docker-compose.dev.yml)

Please note that you will need to alter some variables (`${...:-default}`) to fit your needs. Refer [here](Configuration.md) for further info about configuration.

Once you’ve created the compose files, the second step is to run the Docker deployment. To do so, save these compose files and run:

```sh
docker stack deploy unidash -c docker-compose.yml -c docker-compose.dev.yml -c docker-compose.traefik.yml
```

## Deploy the Client App
You can deploy the client app in many ways since it’s just a Single Page Application and runs anywhere you can host static files.

### Within the Docker Stack
TODO

### With Docker
```sh
docker run -d -p 80:5000 --restart always --name unidash_app unidash/app
```

You can now access the client app by accessing port 80.

### Static Host
TODO