if (!localStorage.getItem("sass")) compile();
else document.addEventListener("load", compile);

async function compile() {
  const sassElem = document.createElement("script");
  sassElem.type = "text/javascript";
  sassElem.src =
    "https://cdnjs.cloudflare.com/ajax/libs/sass.js/0.10.11/sass.min.js";
  sassElem.onload = () => {
    const compiler = new Sass();
    const compile = (text) => new Promise(function (resolve, reject) {
      compiler.compile(text, result => resolve(result));
    });
    const css = "";
    for (const link of document.querySelectorAll(`link[type="text/scss"]`)) {
      css += await compile(await new Request(link.href).text());
    }
  };
}

function load() {}
