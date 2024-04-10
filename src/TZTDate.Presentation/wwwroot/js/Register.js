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

var config = {
    cUrl: "https://api.countrystatecity.in/v1/countries",
    ckey: "NHhvOEcyWk50N2Vna3VFTE00bFp3MjFKR0ZEOUhkZlg4RTk1MlJlaA==",
};

var countrySelect = document.querySelector(".country"),
    stateSelect = document.querySelector(".state"),
    citySelect = document.querySelector(".city");

function loadCountries() {
    let apiEndPoint = config.cUrl;

    fetch(apiEndPoint, { headers: { "X-CSCAPI-KEY": config.ckey } })
        .then((response) => response.json())
        .then((data) => {
            data.forEach((country) => {
                const option = document.createElement("option");
                option.setAttribute('data-iso', country.iso2);
                option.value = country.name;
                option.textContent = country.name;
                countrySelect.appendChild(option);
            });
        })
        .catch((error) => console.error("Error loading countries:", error));

    stateSelect.disabled = true;
    citySelect.disabled = true;
    stateSelect.style.pointerEvents = "none";
    citySelect.style.pointerEvents = "none";
}

function loadStates() {
    stateSelect.disabled = false;
    citySelect.disabled = true;
    stateSelect.style.pointerEvents = "auto";
    citySelect.style.pointerEvents = "none";

    const selectedCountryCode = countrySelect.options[countrySelect.selectedIndex].getAttribute('data-iso');
    stateSelect.innerHTML = '<option value="">Select State</option>'; 

    fetch(`${config.cUrl}/${selectedCountryCode}/states`, {
        headers: { "X-CSCAPI-KEY": config.ckey },
    })
        .then((response) => response.json())
        .then((data) => {
            data.forEach((state) => {
                const option = document.createElement("option");
                option.setAttribute('data-iso', state.iso2);
                option.value = state.name;
                option.textContent = state.name;
                stateSelect.appendChild(option);
            });
        })
        .catch((error) => console.error("Error loading states:", error));
}

function loadCities() {
    citySelect.disabled = false;
    citySelect.style.pointerEvents = "auto";

    const selectedCountryCode = countrySelect.options[countrySelect.selectedIndex].getAttribute('data-iso');
    const selectedStateCode = stateSelect.options[stateSelect.selectedIndex].getAttribute('data-iso');

    citySelect.innerHTML = '<option value="">Select City</option>';

    fetch(
        `${config.cUrl}/${selectedCountryCode}/states/${selectedStateCode}/cities`,
        { headers: { "X-CSCAPI-KEY": config.ckey } }
    )
        .then((response) => response.json())
        .then((data) => {
            data.forEach((city) => {
                const option = document.createElement("option");
                option.setAttribute('data-iso', city.iso2);
                option.value = city.name;
                option.textContent = city.name;
                citySelect.appendChild(option);
            });
        })
        .catch((error) => console.error("Error loading cities:", error));
}

window.onload = loadCountries;
