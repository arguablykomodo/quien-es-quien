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

const scene = new THREE.Scene();
const root = new THREE.Object3D();
scene.add(root);

const camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 1, 2000);
camera.position.z = 600;
scene.add(camera);

const ambientLight = new THREE.AmbientLight(0xaaaaaa, 0.4);
scene.add(ambientLight);
const pointLight = new THREE.PointLight(0xdddddd, 0.8);
camera.add(pointLight);

const characters = [];

function loadModel(character, path, materials) {
    const obj = new THREE.OBJLoader2();
    obj.setLogging(false);
    obj.setMaterials(materials);
    obj.load("../Content/CharacterModels/" + path, e => {
        const object = e.detail.loaderRootNode;
        object.position.x = object.position.y = object.position.z = 100;
        character.add(object);
    });
}

function loadCharacter(el, materials) {
    const character = new THREE.Object3D();
    character.xSpeed = (Math.random() - 0.5) / 500;
    character.ySpeed = (Math.random() - 0.5) / 500;
    character.zSpeed = (Math.random() - 0.5) / 500;

    character.position.x = Math.random() * 500 - 250;
    character.position.y = Math.random() * 500 - 250;
    character.position.z = Math.random() * 500 - 250;

    character.rotation.x = Math.random() * 500 - 250;
    character.rotation.y = Math.random() * 500 - 250;
    character.rotation.z = Math.random() * 500 - 250;

    el.querySelectorAll("model").forEach(model => {
        loadModel(character, model.innerText, materials);
    });
    root.add(character);
    characters.push(character);
}

new THREE.MTLLoader()
    .setPath("../Content/CharacterModels/")
    .load("materials.mtl", function (materials) {
        materials.preload();
        document.querySelectorAll("#characters character").forEach(el => {
            loadCharacter(el, materials.materials);
        });
    })

const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(window.innerWidth, window.innerHeight);
document.getElementById("models").appendChild(renderer.domElement);

let xCamera = 0;
let yCamera = 0;
document.addEventListener("mousemove", e => {
    xCamera += e.movementX;
    yCamera += e.movementY;
});

let scaleSpeed = 0;
document.addEventListener("wheel", e => {
    scaleSpeed += e.deltaY > 0 ? -1 : 1;
    console.log(1 + scaleSpeed)
})

function animate() {
    requestAnimationFrame(animate);
    camera.lookAt(scene.position);
    renderer.render(scene, camera);

    for (const character of characters) {
        character.rotation.x += character.xSpeed;
        character.rotation.y += character.ySpeed;
        character.rotation.z += character.zSpeed;
    }

    root.rotation.y += xCamera / 10000;
    root.rotation.y *= 0.95;
    root.rotation.x += yCamera / 10000;
    root.rotation.x *= 0.95;

    root.scale.x *= 1 + scaleSpeed / 100;
    root.scale.y *= 1 + scaleSpeed / 100;
    root.scale.z *= 1 + scaleSpeed / 100;
    scaleSpeed *= 0.9;
}
animate();
