FROM mcr.microsoft.com/dotnet/sdk:6.0.401-bullseye-slim AS base
COPY . /src
WORKDIR /src

FROM base AS build
RUN dotnet restore && \
    dotnet build --no-restore -c Release

FROM build AS tests
RUN dotnet tests -c Release --no-build --verbosity normal

FROM build AS pack
ARG GITHUB_TAG
ENV GITHUB_TAG=$GITHUB_TAG
RUN chmod +x pack.sh
CMD [ "./pack.sh" ]
