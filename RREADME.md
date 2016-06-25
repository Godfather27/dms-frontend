# LimeSearch | MMT-B2014

# How it works

### Frontend (HTML, JS)
The frontend calls the api and displays the images based on the api's response data.

### Backend (NodeJS)
The backend defines routes for the frontend and offers an api to query images.
The Backend opens .net programms with parameters received from the frontend.


### Service (C#)
There are 2 .net programms:
- text.exe (lucene lib)
	- makes index of meta data
	- query: returns images as JSON
- bildsuche.exe (accord lib)
	- creates bag of words
	- calculates feature vectores on basis of bag of words (devset/img, ~9000)
	- query: returns 10 most similar images as JSON

The programmes are called from the backend with the associated query parameters.

# Get Started
- [Install NodeJS](https://nodejs.org/en/ "Node.js")
- Install .Net / C#

install node modules:
```bash
npm install
```
generate index:
```bash
mono text.exe
```

train bag of words:
```bash
mono bildsuche.exe
```

start the system:
```bash
npm start
```

open [localhost:3000](localhost:3000 "localhost:3000")

# Features of the System
- text search
- image upload
- similar images 

### Team
- Sebastian Huber (FHS37229)
- Konrad Kleeberger (FHS37232)
- Josef Krabath (FHS37235)
- Daniel Trojer (FHS37373)