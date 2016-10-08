FROM microsoft/nanoserver
ENV CONSUL_VERSION 0.7.0

ADD ./dist/consul/consul.exe .
ADD ./dist/webui/ /dist/
ADD ./config /config/

EXPOSE 8300 8301 8301/udp 8302 8302/udp 8400 8500 53 53/udp
# VOLUME ["/data"]

ENTRYPOINT [ "\\consul.exe", "agent", "-config-dir", "/config" ]
