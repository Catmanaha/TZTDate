var lessButton = document.querySelector('.expand-less');
var moreButton = document.querySelector('.expand-more');
var moreDiv = document.querySelector('.expand-more-div');

lessButton.addEventListener('click', () => {
    lessButton.style.display = 'none';
    moreButton.style.display = 'block';
    moreDiv.style.display = 'block';
})

moreButton.addEventListener('click', () => {
    moreButton.style.display = 'none';
    moreDiv.style.display = 'none';
    lessButton.style.display = 'block';
})