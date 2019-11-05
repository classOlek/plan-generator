function addNoClassesBefore(elementId, parentId, day, hour) {
    var element = document.createElement("li");
    element.id = `listItem${elementId}`;
    element.classList.add('list-group-item');
    element.classList.add('noClassesBefore');
    element.innerHTML = `<div class="container"><div class="row">
                        <div class="col" style="font-size: 12px;">
                            No classes before
                        </div>
                        <div class="col-md-3">
                            <select class="form-control" id="timeSelect`+ elementId + `">
                                <option>8:00</option>
                                <option>9:00</option>
                                <option>10:00</option>
                                <option>11:00</option>
                                <option>12:00</option>
                                <option>13:00</option>
                                <option>14:00</option>
                                <option>15:00</option>
                                <option>16:00</option>
                                <option>17:00</option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <select class="form-control" id="daySelect`+ elementId + `">
                                <option>Everyday</option>
                                <option>Monday</option>
                                <option>Tuesday</option>
                                <option>Wednesday</option>
                                <option>Thursday</option>
                                <option>Friday</option>
                            </select>
                        </div>
                        <div class="col-md-1">
                            <button id="removeElement" class="btn btn-danger" onclick="removeElement('listItem`+ elementId + `');">X</button>
                        </div>
                    </div></div>`;
    document.getElementById(parentId).appendChild(element);
    document.getElementById(`timeSelect${elementId}`).value = hour;
    document.getElementById(`daySelect${elementId}`).value = day;
};

function addNoClassesAfter(elementId, parentId, day, hour) {
    var element = document.createElement("li");
    element.id = `listItem${elementId}`;
    element.classList.add('list-group-item');
    element.classList.add('noClassesAfter');
    element.innerHTML = `<div class="container"><div class="row">
                        <div class="col" style="font-size: 12px;">
                            No classes after
                        </div>
                        <div class="col-md-3">
                            <select class="form-control" id="timeSelect`+ elementId + `">
                                <option>8:00</option>
                                <option>9:00</option>
                                <option>10:00</option>
                                <option>11:00</option>
                                <option>12:00</option>
                                <option>13:00</option>
                                <option>14:00</option>
                                <option>15:00</option>
                                <option>16:00</option>
                                <option>17:00</option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <select class="form-control" id="daySelect`+ elementId + `">
                                <option>Everyday</option>
                                <option>Monday</option>
                                <option>Tuesday</option>
                                <option>Wednesday</option>
                                <option>Thursday</option>
                                <option>Friday</option>
                            </select>
                        </div>
                        <div class="col-md-1">
                            <button id="removeElement" class="btn btn-danger" onclick="removeElement('listItem`+ elementId + `');">X</button>
                        </div>
                    </div></div>`;
    document.getElementById(parentId).appendChild(element);
    document.getElementById(`timeSelect${elementId}`).value = hour;
    document.getElementById(`daySelect${elementId}`).value = day;
};