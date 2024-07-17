FROM nginx:alpine

WORKDIR /etc/nginx/conf.d
COPY webgl.conf default.conf

WORKDIR /UnityWebGLBuilds
COPY UnityWebGLBuilds/ .
