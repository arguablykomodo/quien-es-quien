if (!localStorage.getItem("css")) compile();
else document.onload = compile;

async function compile() {
    console.log("hello");
    const sassElem = document.createElement("script");
    sassElem.type = "text/javascript";
    sassElem.src =
        "https://cdnjs.cloudflare.com/ajax/libs/sass.js/0.10.11/sass.min.js";
    document.body.appendChild(sassElem);
    sassElem.onload = async () => {
        const compiler = new Sass("Scripts/sass.worker.js");
        compiler.options({ style: Sass.style.compressed }, () => { });
        const compile = (text) => new Promise(function (resolve, reject) {
            compiler.compile(text, result => resolve(result));
        });
        let css = "";
        for (const link of document.querySelectorAll(`link[type="text/scss"]`)) {
            console.log("link", link.href);
            const request = new Request(link.href);
            const scss = await request.text();
            console.log("css", scss);
            css += JSON.stringify(await compile(scss));
        }
        localStorage.setItem("css", css);
        load();
    };
}

function load() {
    const el = document.createElement("style");
    el.innerText = localStorage.getItem("css");
    document.body.appendChild(el);
}
