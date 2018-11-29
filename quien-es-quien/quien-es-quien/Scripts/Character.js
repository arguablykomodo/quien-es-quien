const scene = new THREE.Scene();

const camera = new THREE.PerspectiveCamera(45, 250 / 300, 1, 2000);
camera.position.z = 150;
scene.add(camera);

const ambientLight = new THREE.AmbientLight(0xaaaaaa, 0.4);
scene.add(ambientLight);
const pointLight = new THREE.PointLight(0xdddddd, 0.8);
camera.add(pointLight);

const root = new THREE.Object3D();
scene.add(root);
const models = [];

function setModel(i, el, materials) {
    const path = el.selectedOptions[0].getAttribute("data-path");
    const obj = new THREE.OBJLoader2();
    obj.setLogging(false);
    obj.setMaterials(materials);
    obj.load("../Content/CharacterModels/" + path, e => {
        console.log(e);
        const object = e.detail.loaderRootNode;
        const oldObject = models[i];
        if (oldObject) root.remove(oldObject);
        root.add(object);
        models[i] = object;
    })
}

new THREE.MTLLoader()
    .setPath("../Content/CharacterModels/")
    .load("materials.mtl", function (materials) {
        materials.preload();
        document.querySelectorAll("select").forEach((el, i) => {
            setModel(i, el, materials.materials);
            el.addEventListener("change", () => setModel(i, el, materials.materials))
        });
    })

const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(250, 300);
document.getElementById("helios").appendChild(renderer.domElement);

let speed = 0;
let otherSpeed = 0;
document.addEventListener("mousemove", e => {
    speed += e.movementX;
    otherSpeed += e.movementY;
});

function animate() {
    requestAnimationFrame(animate);
    camera.lookAt(scene.position);
    renderer.render(scene, camera);
    root.rotation.y += speed / 1000;
    root.rotation.y *= 0.975;
    root.rotation.x += otherSpeed / 1000;
    root.rotation.x *= 0.975;
    speed *= 0.9;
    otherSpeed *= 0.9;
}
animate();
