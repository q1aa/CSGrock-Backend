const portInput = document.getElementById('port-select');
const copycmdInput = document.getElementById('copy-cmd');
const selectplatformInput = document.getElementById('select-platform');
const frameworkSelect = document.getElementById('framework-select');

portInput.addEventListener('change', function() {
    changeCopyCMDText(this.value);
    frameworkSelect.value = 'None';
});

copycmdInput.addEventListener('click', function() {
    this.select();
    document.execCommand('copy');
});

frameworkSelect.addEventListener('change', function() {
    const platform = this.value;
    console.log(platform);

    switch (platform) {
        /*Laravel Djnago Flask Express Ruby ASP.NET Spring Hugo*/
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
}