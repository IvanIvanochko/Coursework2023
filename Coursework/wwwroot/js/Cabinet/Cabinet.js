
const buttons = document.querySelectorAll('button[name="myButton"]');
const icons = document.querySelectorAll('img[name="myIcon"]');
const inputs = document.querySelectorAll('input[class="myInput"]');

inputs.forEach((input) => {
    input.readOnly = true;
});

buttons.forEach((button, index) => {
    button.addEventListener('mouseover', () => {
        icons[index].style.filter = 'brightness(0) saturate(100%) invert(45%) sepia(0%) saturate(2406%) hue-rotate(312deg) brightness(96%) contrast(98%)';
    });

    button.addEventListener('mouseout', () => {
        icons[index].style.filter = '';
    });

    button.addEventListener('click', () => {
        if (inputs[index].readOnly === true) {
            inputs[index].readOnly = false;

            const computedStyle = getComputedStyle(inputs[index]);
            const outlineValue = computedStyle.getPropertyValue('outline');

            console.log(outlineValue);

            inputs[index].style.backgroundColor = "rgba(0, 0, 28, 0.1)";
            inputs[index].style.outline = 'rgb(0, 0, 0) solid 3px';

            icons[index].src = "../vectors-svg/success-check-win-done-mark-good-svgrepo-com.svg";
        }
        else {
            inputs[index].readOnly = true;

            inputs[index].style.backgroundColor = "rgba(0, 0, 0, 0)";
            inputs[index].style.outline = 'rgb(255, 255, 255) none 0px';

            icons[index].src = "../vectors-svg/edit-button-svgrepo-com.svg";
        }

    });
});

const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
        if (entry.isIntersecting) {
            entry.target.classList.add('show');
        }
        else {
            entry.target.classList.remove('show');
        }
    });
});

const hiddenElements = document.querySelectorAll('.hidden');
hiddenElements.forEach((el) => observer.observe(el));

const observer_pop = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
        if (entry.isIntersecting) {
            entry.target.classList.add('show-pop');
        }
        else {
            entry.target.classList.remove('show-pop');
        }
    });
});

const hiddenElements_pop = document.querySelectorAll('.hidden-pop');
hiddenElements_pop.forEach((el) => observer_pop.observe(el));



const boxPopUp = document.getElementById('boxPopUp'); // openBoxPopUpBtn

function openBoxPopUp() {
    boxPopUp.classList.add('box-open');
}

function closeBoxPopUp() {
    console.log("closeBoxPopUp function called");
    boxPopUp.classList.remove('box-open');

    const dataElement = document.getElementById('data');

    // Parse the data from the hidden element
    console.log(userInfo);


    const data = {};

    inputs.forEach((input) => {
        const name = input.name;
        const value = input.value;
        data[name] = value;
    });

    // Make a POST request to the server
    fetch('/cabinet', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                location.reload();
                // Handle the server response if needed
            } else {
                throw new Error('Data sending failed');
            }
        })
        .catch(error => {
            console.error('Error sending data:', error);
        });
}

const passPopUp = document.getElementById('passPopUp');

function openPassPopUp() {
    passPopUp.classList.add('box-open');
}

function closePassPopUp() {
    passPopUp.classList.remove('box-open');
}

const passInputOld = document.getElementById('passInputOld');
const passInputNew1 = document.getElementById('passInputNew1');
const passInputNew2 = document.getElementById('passInputNew2');
const passPopUpP = document.getElementById('passPopUpP');
const passPopUpP_Error = document.getElementById('passPopUpP_Error');
const passPopUpImg = document.getElementById('passPopUpImg');
const passPopUpBtnP = document.getElementById('passPopUpBtnP');
const passPopUpBtn = document.getElementById('passPopUpBtn');

function verifyPassPopUp() {
    if (passInputOld.value === "" || passInputNew1.value === "" || passInputNew2.value === "") {
        passPopUpP_Error.innerHTML = "Ви не ввели всі дані";
        return;
    }

    if (userInfo.Password !== passInputOld.value) {
        passPopUpP_Error.innerHTML = "Ваш старий пароль не співпав";
        return;
    }

    if (passInputNew1.value !== passInputNew2.value) {
        passPopUpP_Error.innerHTML = "Нові паролі не співпадають";
        return;
    }

    if (userInfo.Password === passInputNew1.value) {
        passPopUpP_Error.innerHTML = "Новий пароль співпадає з старим";
        return;
    }

    passPopUpP_Error.innerHTML = "";
    passPopUpImg.src = "../vectors-svg/password-icons/password-pass-svgrepo-com.svg";
    passPopUpBtnP.innerHTML = 'Пароль успішно змінено!';
    passPopUpBtn.disabled = true;
    closePassPopUp();

    const data = {};

    inputs.forEach((input) => {
        const name = input.name;
        const value = input.value;
        data[name] = value;
    });
    data['Password'] = passInputNew1.value;

    fetch('/Home/UpdateUserPassword', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                console.log('Data sent successfully');

                // Handle the server response if needed
            } else {
                throw new Error('Data sending failed');
            }
        })
        .catch(error => {
            console.error('Error sending data:', error);
        });
}

const passPopUpRow = document.getElementById('passPopUpRow');
const marginLeft = ((passPopUpRow.clientWidth - passPopUpP.clientWidth) / 2) / (window.innerWidth / 100); // Calculate margin in vw

passPopUpP.style.marginLeft = marginLeft + 'vw';

const BirthdateInput = document.getElementById('BirthdateInput');

BirthdateInput.valueAsDate = userInfo.Birthdate ? new Date(userInfo.Birthdate) : '';