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
