const style = document.createElement("style");
document.body.append(style);
if (localStorage.getItem(`css-${location.pathname}`)) {
    style.innerHTML = localStorage.getItem(`css-${location.pathname}`);
    window.addEventListener("load", doEverything);
} else {
    document.addEventListener("DOMContentLoaded", doEverything);
}

function doEverything() {
    Sass.setWorkerUrl("/Scripts/worker.js");
    const sass = new Sass();
    sass.options({ style: Sass.style.compressed }, () => { });

    function compile(url) {
        return new Promise(r => {
            fetch(url)
                .then(response => response.text())
                .then(text =>
                    sass.compile(text, function callback(scss) {
                        r(scss.text);
                    })
                );
        });
    }

    const promises = [];
    for (const el of document.querySelectorAll('[type="text/scss"]')) {
        promises.push(compile(el.getAttribute("href")));
    }

    Promise.all(promises).then(results => {
        const css = results.join("\n");
        localStorage.setItem(`css-${location.pathname}`, css);
        style.innerHTML = css;
    });
}
