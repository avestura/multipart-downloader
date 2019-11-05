# MultiPart Downloader

This is a sample socket proramming project which serves a file in server, and client can ask server to chunk the files and serve each part separately.

### How does it work?

1. Run server and provide a file path to it. Now server serves the file for any client who request it, asynchronously.
2. Run as many as clients you want. Each client should ask server how many parts it want, and at every stage provide a partition (chunk) number to server.
3. When you download all the parts, client automatically merges the parts and dinner is served!



By: Aryan (Hossein) EbrahimPour -- Student ID: 98723249

Built in [Iran University of Science and Technology](https://iust.ac.ir) without 💖


## Screenshot

![Screenshot](scr.jpg)


## Build and Run

First clone this repo

```bash
git clone https://github.com/0xaryan/multipart-downloader
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