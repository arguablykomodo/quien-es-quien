let i = 0;
let el = document.getElementById("text");
let text = document.getElementById("text").getAttribute("data-text");

function letter() {
    el.textContent += text[i];
    i++;
    if (i >= text.length) {
        return;
    }
    setTimeout(letter, text[i-1] === "," ? 200 : 50)
}

letter();
