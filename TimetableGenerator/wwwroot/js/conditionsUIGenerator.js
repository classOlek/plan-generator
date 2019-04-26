function addNoClassesBefore(elementId, parentId, day, hour) {
    var element = document.createElement("li");
    element.id = `listItem${elementId}`;
    element.classList.add('list-group-item');
    element.classList.add('noClassesBefore');
    element.innerHTML = `<div class="row">
                        <div class="col-md-7">
                            <div class="form-group row">
                                <label for="inputEmail3" class="col-sm-6 col-form-label">No classes before</label>
                                <div class="col-sm-4">
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
                            </div>
                        </div>
                        <div class="col-md-3">
                            <select class="form-control" id="daySelect`+ elementId + `">
                                <option>Everyday</option>
                                <option>Monday</option>
                                <option>Tuesday</option>
                                <option>Wednesday</option>
                                <option>Thursday</option>
                                <option>Friday</option>
                            </select>
                        </div>
                        <div class="col-md-2">
                            <button id="removeElement" class="btn btn-danger" onclick="removeElement('listItem`+ elementId + `');">X</button>
                        </div>
                    </div>`;
    document.getElementById(parentId).appendChild(element);
    document.getElementById(`timeSelect${elementId}`).value = hour;
    document.getElementById(`daySelect${elementId}`).value = day;
};

function addNoClassesAfter(elementId, parentId, day, hour) {
    var element = document.createElement("li");
    element.id = `listItem${elementId}`;
    element.classList.add('list-group-item');
    element.classList.add('noClassesAfter');
    element.innerHTML = `<div class="row">
                        <div class="col-md-7">
                            <div class="form-group row">
                                <label for="inputEmail3" class="col-sm-6 col-form-label">No classes after</label>
                                <div class="col-sm-4">
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
                            </div>
                        </div>
                        <div class="col-md-3">
                            <select class="form-control" id="daySelect`+ elementId + `">
                                <option>Everyday</option>
                                <option>Monday</option>
                                <option>Tuesday</option>
                                <option>Wednesday</option>
                                <option>Thursday</option>
                                <option>Friday</option>
                            </select>
                        </div>
                        <div class="col-md-2">
                            <button id="removeElement" class="btn btn-danger" onclick="removeElement('listItem`+ elementId + `');">X</button>
                        </div>
                    </div>`;
    document.getElementById(parentId).appendChild(element);
    document.getElementById(`timeSelect${elementId}`).value = hour;
    document.getElementById(`daySelect${elementId}`).value = day;
};