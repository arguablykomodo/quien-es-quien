const type = document.querySelector("select[name=type]");
const char = document.querySelector("select[name=characteristic]");
const chars = Array.from(document.querySelectorAll("select[name=characteristic] option"));

function filter() {
    chars.forEach(e => {
        if (e.getAttribute("data-type") === type.selectedOptions[0].getAttribute("value")) {
            e.style.display = "";
        } else {
            e.style.display = "none";
        }
    });
    char.selectedIndex = chars.findIndex(e => e.getAttribute("data-type") === type.selectedOptions[0].getAttribute("value"));
}

type.addEventListener("change", filter);
filter();
