const camera = new THREE.PerspectiveCamera(45, 250 / 500, 1, 2000);
camera.position.z = 250;

const scene = new THREE.Scene();

const ambientLight = new THREE.AmbientLight(0xaaaaaa, 0.4);
scene.add(ambientLight);

const pointLight = new THREE.PointLight(0xdddddd, 0.8);
camera.add(pointLight);
scene.add(camera);

let helios;
const loader = new THREE.OBJLoader();
loader.load("../Content/helios.obj", obj => {
    obj.traverse(function (child) {
        if (child instanceof THREE.Mesh) {
            child.material = new THREE.MeshPhongMaterial({ color: 0xaaaaaa });
        }
    });
    helios = obj;
    scene.add(helios);
});

const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(250, 500);
document.getElementById("helios").appendChild(renderer.domElement);

let speed = 0;
document.addEventListener("mousemove", e => {
    speed += e.movementX;
});

function animate() {
    requestAnimationFrame(animate);
    camera.lookAt(scene.position);
    renderer.render(scene, camera);
    if (helios) {
        helios.rotation.y += 0.01 + speed / 1000;
        speed *= 0.9;
    }
}
animate();
