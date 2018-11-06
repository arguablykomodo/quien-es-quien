const camera = new THREE.PerspectiveCamera(45, 1, 1, 2000);
camera.position.z = 5;

const scene = new THREE.Scene();

const ambientLight = new THREE.AmbientLight(0xaaaaaa, 0.4);
scene.add(ambientLight);

const pointLight = new THREE.PointLight(0xdddddd, 0.8);
camera.add(pointLight);
scene.add(camera);

const loader = new THREE.OBJLoader();
let title;
loader.load("../Content/title.obj", obj => {
    obj.traverse(function (child) {
        if (child instanceof THREE.Mesh) {
            child.material = new THREE.MeshPhongMaterial({ color: 0x91e9ff });
        }
    });
    title = obj;
    scene.add(title);
});

/*let question;
loader.load("../Content/question.obj", obj => {
    obj.traverse(function (child) {
        if (child instanceof THREE.Mesh) {
            child.material = new THREE.MeshPhongMaterial({ color: 0x01cdfe });
        }
    });
    question = obj;
    scene.add(question);
});*/

const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(300, 300);
document.getElementById("title").appendChild(renderer.domElement);

let speed = 0;
document.addEventListener("mousemove", e => {
    speed += e.movementX;
});

function animate() {
    requestAnimationFrame(animate);
    camera.lookAt(scene.position);
    renderer.render(scene, camera);
    if (title) {
        title.rotation.y += 0.01 + speed / 1000;
    }
    /*if (question) {
        question.rotation.y += 0.01 + speed / 1000;
    }*/
    speed *= 0.9;
}
animate();
