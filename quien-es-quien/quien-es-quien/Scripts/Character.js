const scene = new THREE.Scene();

const camera = new THREE.PerspectiveCamera(45, 250 / 300, 1, 2000);
camera.position.z = 150;
scene.add(camera);

const ambientLight = new THREE.AmbientLight(0xaaaaaa, 0.4);
scene.add(ambientLight);
const pointLight = new THREE.PointLight(0xdddddd, 0.8);
camera.add(pointLight);

const models = [];
const obj = new THREE.OBJLoader2();
new THREE.MTLLoader()
    .setPath("../Content/CharacterModels/")
    .load("materials.mtl", function (materials) {
        materials.preload();
        obj.setMaterials(materials);
        for (const el of document.querySelectorAll("select")) {
            const path = el.selectedOptions[0].getAttribute("data-path");

            obj.load("../Content/CharacterModels/" + path, function (event) {
                const object = event.detail.loaderRootNode;
                object.position.x = 0;
                object.position.y = 0;
                object.position.z = 0;
                console.log(object)
                models.push(object);
                scene.add(object);
            });
        }
    })

const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(250, 300);
document.getElementById("helios").appendChild(renderer.domElement);

let speed = 0;
document.addEventListener("mousemove", e => {
    speed += e.movementX;
});

function animate() {
    requestAnimationFrame(animate);
    camera.lookAt(scene.position);
    renderer.render(scene, camera);
    for (const model of models) {
        model.rotation.y += 0.001 + speed / 1000;
    }
    speed *= 0.9;
}
animate();
