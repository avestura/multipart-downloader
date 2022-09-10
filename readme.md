# MultiPart Downloader

> **Warning**
> This is a custom TCP protocol for demonstration purposes only and most definitely suffers from security and efficiency issues. As a result, using this protocol for a real world scenario is not planned, nor recommended.

This is a sample socket proramming project which serves a file in server, and client can ask server to chunk the files and serve each part separately.

### How does it work?

1. Run server and provide a file path to it. Now server serves the file for any client who request it, asynchronously.
2. Run as many as clients you want. Each client should ask server how many parts it want, and at every stage provide a partition (chunk) number to server.
3. When you download all the parts, client automatically merges the parts and dinner is served!



By: Aryan EbrahimPour

Built in [Iran University of Science and Technology](https://iust.ac.ir)


## Screenshot

![Screenshot](scr.jpg)


## Build and Run

First clone this repo

```bash
git clone https://github.com/avestura/multipart-downloader
```

Then use dotnet cli commands

```
cd server
dotnet build
dotnet run

cd ..\client
dotnet build
dotnet run
```
