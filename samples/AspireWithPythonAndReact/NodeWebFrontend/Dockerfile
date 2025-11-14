FROM node:20 AS build

WORKDIR /app

COPY package.json package.json
COPY package-lock.json package-lock.json

RUN npm install

COPY . .

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

# Set default environment variables for nginx template
ENV PORT=8000
ENV APISERVICE_BASE_URL=http://localhost:8080

EXPOSE 8000

CMD ["nginx", "-g", "daemon off;"]