﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const ALERT_SUCCESS = "alert-success";
const ALERT_ERROR = "alert-error";
const ALERT_ANIMATION_TIMEOUT = 4000;
const ALERT_REMOVE_TIMEOUT = 500;

// Initialize text fields
const textFields = document.querySelectorAll('.mdc-text-field');
textFields.forEach((textField) => {
    new mdc.textField.MDCTextField(textField);
});

const alertContainer = document.querySelector(".alert-container");

const getAlert = (type, msg) => {
    return `
          <div class="alert swing-in-top-fwd ${type}">
            ${msg}
          </div>
          `
}

const showAlert = (html) => {
    let alert = document.createElement("div");
    alert.innerHTML = html;
    alertContainer.appendChild(alert);

    setTimeout(() => {
        alert.classList.add("swing-out-top-bck");
    }, ALERT_ANIMATION_TIMEOUT);

    setTimeout(() => {
        alertContainer.removeChild(alert);
    }, ALERT_ANIMATION_TIMEOUT + ALERT_REMOVE_TIMEOUT);
}