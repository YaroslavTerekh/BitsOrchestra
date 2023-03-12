let receivedData = [];
let index = 1;
let table;

generateTable();

function deleteContact(obj) {
    $.ajax({
        method: "DELETE",
        url: obj,
        async: true,
        success: newData => {
            table.destroy();
            generateTable();
        }
    })
}

function editContact(id) {
    let inputs = $(`.a${id}`);

    let request = {};

    inputs.each((el, value) => {
        let inputId = value.attributes.getNamedItem('id').value;

        switch (inputId) {
            case 'nameInput':
                request.name = value.value;
                break;
            case 'phoneInput':
                request.phone = value.value;
                break;
            case 'dateInput':
                request.dateOfBirth = value.value;
                break;
            case 'salaryInput':
                request.salary = parseInt(value.value);
                break;
            case 'isMarried':
                request.married = value.checked;
                break;
            default:
                break;
        }
    })

    $.ajax({
        method: 'POST',
        url: `contact/${id}/modify`,
        async: true,
        data: request,
        success: newData => {
            table.destroy();
            generateTable();
        },
        error: error => {
            error.responseJSON.forEach(val => {
                $('#invalid-state').empty();
                const $newError = $(`<li>${val[0].errorMessage}</li>`);
                $('#invalid-state').append($newError);
            });    
        }
    })
}

function generateTable() {
    $.ajax({
        type: "GET",
        url: "allcontacts",
        async: true,
        success: newData => {
            receivedData = [];
            index = 1;
            $.each(newData, (key, val) => {
                let isChecked = val.married ? 'checked' : '';
                let dateString = new Date(val.dateOfBirth).toISOString().slice(0, 10);

                let nameInput = `<input class='contact-input a${val.id}' id='nameInput' type='text' value='${val.name}' />`;
                let dateInput = `<input class='contact-input a${val.id}' id='dateInput' type='date' value=${dateString} />`;
                let phoneInput = `<input class='contact-input a${val.id}' id='phoneInput' type='text' value=${val.phone} />`;
                let salaryInput = `<input class='contact-input a${val.id}' id='salaryInput' type='text' value=${val.salary} />`;
                let isMarriedCheckbox = `<input type='checkbox' class='a${val.id}' id='isMarried' ${isChecked} />`;
                let editButton = `<button class='edit' onclick=editContact('${val.id}')>Edit</button>`;
                let deleteButton = `<button class='delete' onclick="deleteContact('${val.id}')">Delete</button>`;
                let actions = `${editButton} | ${deleteButton}`;

                receivedData.push([index++, nameInput, dateInput, isMarriedCheckbox, phoneInput, salaryInput, new Date(val.createdTime).toLocaleDateString(), actions]);
            });

            $.fn.dataTable.ext.order['dom-text'] = function (settings, col) {
                return this.api()
                    .column(col, { order: 'index' })
                    .nodes()
                    .map(function (td, i) {
                        return $('input', td).val();
                    });
            };

            $.fn.dataTable.ext.order['dom-text-numeric'] = function (settings, col) {
                return this.api()
                    .column(col, { order: 'index' })
                    .nodes()
                    .map(function (td, i) {
                        return $('input', td).val() * 1;
                    });
            };

            $.fn.dataTable.ext.order['dom-select'] = function (settings, col) {
                return this.api()
                    .column(col, { order: 'index' })
                    .nodes()
                    .map(function (td, i) {
                        return $('select', td).val();
                    });
            };

            $.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
                return this.api()
                    .column(col, { order: 'index' })
                    .nodes()
                    .map(function (td, i) {
                        return $('input', td).prop('checked') ? '1' : '0';
                    });
            };

            table = $('#dataTable').DataTable({
                data: receivedData,
                destroy: true,
                columns: [
                    null,
                    { orderDataType: 'dom-text', type: 'string' },
                    { orderDataType: 'dom-text', type: 'string' },
                    { orderDataType: 'dom-checkbox' },
                    { orderDataType: 'dom-text', type: 'string' },
                    { orderDataType: 'dom-text', type: 'number' },
                    null,
                    null
                ],
            });
        }
    });
}