const audio = document.getElementById("audio");
const time = localStorage.getItem("song");
if (time) audio.currentTime = time;
setInterval(() => localStorage.setItem("song", audio.currentTime), 1000);