function setElementFocus(elementClass) {
    let focusElement = document.getElementsByClassName(elementClass)[0];
    focusElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
    let opacity = 1;
    let timer = setInterval(() => {
        if (opacity <= 0) {
            clearInterval(timer);
            focusElement.style.border = "none";
        } else {
            let red = Math.floor(opacity * 255);
            let blue = 255 - red;
            focusElement.style.border = `2px solid rgba(${blue}, 0, ${red}, ${opacity})`;
            opacity -= 0.02;
        }
    }, 100);
}

const portInput = document.getElementById('port-select');
const copycmdInput = document.getElementById('copy-cmd');
const selectplatformInput = document.getElementById('select-platform');
const frameworkSelect = document.getElementById('framework-select');
const frameworkInfoText = document.getElementById('framework-info-text');

document.addEventListener('DOMContentLoaded', function () {
    handleScreenWidth();
});

window.addEventListener('resize', function () {
    handleScreenWidth();
});

function handleScreenWidth() {
    let screenWidth = window.innerWidth;

    if (screenWidth < 1000) document.getElementById('explanation-image').src = 'how-it-works-mobile.png';
    else document.getElementById('explanation-image').src = 'how-it-works-pc.png';
}

portInput.addEventListener('change', function () {
    changeCopyCMDText(this.value);
    frameworkSelect.value = 'None';
});

copycmdInput.addEventListener('click', function () {
    this.select();
    document.execCommand('copy');
});

frameworkSelect.addEventListener('change', function () {
    const platform = this.value;

    switch (platform) {
        case 'Laravel':
            portInput.value = '8000';
            changeCopyCMDText('8000');
            break;
        case 'Django':
            portInput.value = '8000';
            changeCopyCMDText('8000');
            break;
        case 'Flask':
            portInput.value = '5000';
            changeCopyCMDText('5000');
            break;
        case 'Apache / Ngnix':
            portInput.value = '80';
            changeCopyCMDText('80');
            break;
        case 'Ruby':
            portInput.value = '3000';
            changeCopyCMDText('3000');
            break;
        case 'ASP.NET':
            portInput.value = '5000';
            changeCopyCMDText('5000');
            break;
        case 'Spring':
            portInput.value = '8080';
            changeCopyCMDText('8080');
            break;
        case 'Hugo':
            portInput.value = '1313';
            changeCopyCMDText('1313');
            break;
        case 'Next.js':
            portInput.value = '3000';
            changeCopyCMDText('3000');
            break;
        case 'React':
            portInput.value = '3000';
            changeCopyCMDText('3000');
            break;
        default:
            portInput.value = '8000';
            changeCopyCMDText('8000');
            break;
    }
});

function changeCopyCMDText(port) {
    copycmdInput.value = `csgrok http ${port}`;
    frameworkInfoText.innerText = `${frameworkSelect.value} is running on port ${port} by default`;
}