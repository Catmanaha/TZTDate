function previewImage(event, imageId) {
    const imgElement = document.getElementById(imageId);
    const file = event.target.files[0];

    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            imgElement.src = e.target.result;
        };
        reader.readAsDataURL(file);
    }
}

var checkboxes = document.querySelectorAll(
    'input[type="checkbox"][name="interest"]'
);

var interest = document.getElementById("interests");

checkboxes.forEach(function (checkbox) {
    checkbox.addEventListener("change", function () {
        if (checkbox.checked) {
            interest.value += checkbox.value + " ";
        } else {
            var valueToRemove = checkbox.value + " ";
            interest.value = interest.value.replace(valueToRemove, "");
        }
    });
});

const navigateToFormStep = (stepNumber) => {
    document.querySelectorAll(".form-step").forEach((formStepElement) => {
        formStepElement.classList.add("d-none");
    });

    document
        .querySelectorAll(".form-stepper-list")
        .forEach((formStepHeader) => {
            formStepHeader.classList.add("form-stepper-unfinished");
            formStepHeader.classList.remove(
                "form-stepper-active",
                "form-stepper-completed"
            );
        });

    if (stepNumber == 2) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                fetch(
                    `https://api.geoapify.com/v1/geocode/reverse?lat=${position.coords.latitude}&lon=${position.coords.longitude}&format=json&apiKey=da35841dcf8646d39be7dce92c560ed4`
                )
                    .then((response) => {
                        console.log(response);
                        return response.json();
                    })
                    .then((response) => {
                        let allDetails = response.results[0];
                        document.getElementById("country").value =
                            allDetails.country;
                        document.getElementById("city").value = allDetails.city;
                        document.getElementById("postCode").value =
                            allDetails.postcode;
                        document.getElementById("district").value =
                            allDetails.district;
                        document.getElementById("street").value =
                            allDetails.street;
                        document.getElementById("longitude").value =
                            allDetails.lon;
                        document.getElementById("latitude").value =
                            allDetails.lat;
                    })
                    .catch(() => {
                        console.log("Something went wrong");
                    });
            });
        } else {
            x.innerHTML = "Geolocation is not supported by this browser.";
        }
    }

    document.querySelector("#step-" + stepNumber).classList.remove("d-none");

    const formStepCircle = document.querySelector(
        'li[step="' + stepNumber + '"]'
    );

    formStepCircle.classList.remove(
        "form-stepper-unfinished",
        "form-stepper-completed"
    );
    formStepCircle.classList.add("form-stepper-active");

    for (let index = 0; index < stepNumber; index++) {
        const formStepCircle = document.querySelector(
            'li[step="' + index + '"]'
        );

        if (formStepCircle) {
            formStepCircle.classList.remove(
                "form-stepper-unfinished",
                "form-stepper-active"
            );
            formStepCircle.classList.add("form-stepper-completed");
        }
    }
};

document
    .querySelectorAll(".btn-navigate-form-step")
    .forEach((formNavigationBtn) => {
        formNavigationBtn.addEventListener("click", () => {
            const stepNumber = parseInt(
                formNavigationBtn.getAttribute("step_number")
            );

            navigateToFormStep(stepNumber);
        });
    });

document.addEventListener("DOMContentLoaded", function () {
    const input1 = document.getElementById("searchingAgeStart");
    const input2 = document.getElementById("searchingAgeEnd");
    const errorMessage = document.getElementById("errorMessage");
    const nextButton = document.getElementById("next3");

    function handleInputChange() {
        const value1 = parseFloat(input1.value);
        const value2 = parseFloat(input2.value);

        if (value1 > value2) {
            errorMessage.textContent =
                "Age start cannot be greater than age end";
            input1.style.borderColor = "red";
            input2.style.borderColor = "red";
            nextButton.setAttribute("disabled", "disabled");
        } else {
            errorMessage.textContent = "";
            input1.style.borderColor = "";
            input2.style.borderColor = "";
            nextButton.removeAttribute("disabled");
        }
    }

    input1.addEventListener("input", handleInputChange);
    input2.addEventListener("input", handleInputChange);
});

document.addEventListener("DOMContentLoaded", function () {
    const sectionStep1 = document.getElementById("step-1");
    const nextButton = document.getElementById("next1");

    const inputs = sectionStep1.querySelectorAll("input[required], select[required]");

    function checkInputsValidity() {
        let isValid = true;

        inputs.forEach(input => {
            if (!input.checkValidity()) {
                isValid = false;
            }
        });

        return isValid;
    }

    function handleInputChange() {
        if (checkInputsValidity()) {
            nextButton.removeAttribute("disabled");
        } else {
            nextButton.setAttribute("disabled", "disabled");
        }
    }

    inputs.forEach(input => {
        input.addEventListener("input", handleInputChange);
    });

    handleInputChange();
});


document.addEventListener("DOMContentLoaded", function () {
    const sectionStep1 = document.getElementById("step-2");
    const nextButton = document.getElementById("next2");

    const inputs = sectionStep1.querySelectorAll("input[required], select[required]");

    function checkInputsValidity() {
        let isValid = true;

        inputs.forEach(input => {
            if (!input.checkValidity()) {
                isValid = false;
            }
        });

        return isValid;
    }

    function handleInputChange() {
        if (checkInputsValidity()) {
            nextButton.removeAttribute("disabled");
        } else {
            nextButton.setAttribute("disabled", "disabled");
        }
    }

    inputs.forEach(input => {
        input.addEventListener("input", handleInputChange);
    });

    handleInputChange();
});


document.addEventListener("DOMContentLoaded", function () {
    const sectionStep1 = document.getElementById("step-4");
    const saveButton = document.getElementById("save");

    const inputs = sectionStep1.querySelectorAll("input[required], select[required]");

    function checkInputsValidity() {
        let isValid = true;

        inputs.forEach(input => {
            if (!input.checkValidity()) {
                isValid = false;
            }
        });

        return isValid;
    }

    function handleInputChange() {
        if (checkInputsValidity()) {
            saveButton.removeAttribute("disabled");
        } else {
            saveButton.setAttribute("disabled", "disabled");
        }
    }

    inputs.forEach(input => {
        input.addEventListener("input", handleInputChange);
    });

    handleInputChange();
});