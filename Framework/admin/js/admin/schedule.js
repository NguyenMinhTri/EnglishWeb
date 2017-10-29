function AddInput(week) {
	var Id = $("#tuan" + week + " textarea").length + 1;
	var fieldWrapper = $("<div class='form-group' style='display: none;'/>");
	var dropdownlist = $("<label class='col-sm-2 control-label'> Buổi "
			+ Id
			+ " <star>*</star></label> <div class='col-sm-8'><select multiple class='selectpicker col-sm-12' title='Chọn lession' data-style='btn btn-block'><option value='ARS'>ARS</option><option value='ARS'>ARS</option></select></div>");
	var textarea = $("<div class='col-sm-12 textarea-input'><textarea class='form-control' placeholder='Chi tiết...' rows='3' name='buoi"
			+ Id + "' id='buoi" + Id + "'></textarea></div>");
	var removeButton = $("<div class='col-sm-2'><button type='button' class='btn btn-danger btn-sm btn-add-input'><span class='ti-minus'></span></button></div>");
	removeButton.click(function() {
		$(this).parent().slideUp("fast", function() {
			$(this).remove();
			ResetInputId(week);
		});
	});
	fieldWrapper.append(dropdownlist);
	fieldWrapper.append(removeButton);
	fieldWrapper.append(textarea);
	$("#tuan" + week).append(fieldWrapper);
	$("#tuan" + week).find(".form-group:last").slideDown("fast");
	$('.selectpicker').selectpicker({});
}

$()
		.ready(
				function() {
					$("#addPanel")
							.click(
									function() {
										var Id = $("#panel-group-schedule .panel-group-sm").length + 1;
										var fieldWrapper = $("<div class='panel-group-sm' style='display: none;'/>");
										var content = $("<div class='col-sm-7 col-sm-offset-3'><div class='panel panel-border panel-default'><a data-toggle='collapse' href='#collapse"
												+ Id
												+ "'><div class='panel-heading'><h4 class='panel-title'>Tuần "
												+ Id
												+ " <i class='ti-angle-down'></i></h4></div></a><div id='collapse"
												+ Id
												+ "' class='panel-collapse collapse'><div class='panel-body'><div id='tuan"
												+ Id
												+ "'><div class='form-group'><label class='col-sm-2 control-label'>Buổi 1 <star>*</star></label><div class='col-sm-8'><select multiple class='selectpicker col-sm-12' title='Chọn lession' data-style='btn btn-block'><option value='ARS'>ARS</option><option value='ARS'>ARS</option></select></div><div class='col-sm-2'><button type='button' class='btn btn-success btn-sm btn-add-input' onclick = 'AddInput("
												+ Id
												+ ");'><span class='ti-plus'></span></button></div><div class='col-sm-12 textarea-input'><textarea class='form-control' placeholder='Chi tiết...' rows='3' name='buoi"
												+ Id
												+ "' id='buoi"
												+ Id
												+ "'></textarea></div></div></div></div></div></div></div>");

										var removeButton = $("<div class='col-sm-2'><button type='button' class='btn btn-danger btn-add-panel'><span class='ti-minus'></span></button></div>");
										removeButton.click(function() {
											$(this).parent().slideUp("fast",
													function() {
														$(this).remove();
														ResetPanelId();
													});
										});
										fieldWrapper.append(/content);
										fieldWrapper.append(removeButton);
										$("#panel-group-schedule").append(
												fieldWrapper);
										$("#panel-group-schedule").find(
												".panel-group-sm:last")
												.slideDown("fast");
										$('.selectpicker').selectpicker({});
									});
				});

function ResetInputId(week) {
	var Id = 0;
	$("#tuan" + week + " .form-group").each(function() {
		Id++;
		$(this).find("label").html("Buổi " + Id + " <star>*</star>");
	});
}

function ResetPanelId() {
	var Id = 0;
	$("#panel-group-schedule .panel-group-sm").each(
			function() {
				Id++;
				$(this).find("h4").html(
						"Tuần " + Id + " <i class='ti-angle-down'></i>");
			});
}

function POST() {
	var data = $("#form").serializeFormJSON();
	data.id = 0;
	$
			.ajax({
				url : "/SpringRestHibernateExample/addSchedule",
				method : "POST",
				contentType : "application/json",
				data : JSON.stringify(data),
				success : function() {
					Message("Thành công", "Bạn đã thêm dữ liệu thành công!",
							"success");
					$('#bootstrap-table').bootstrapTable('refresh');
					ResetInput();
				},
				error : function() {
					Message("Thất bại", "Đã có lỗi xảy ra!", "danger");
				}
			});
}

function PUT() {
	var data = $("#form").serializeFormJSON();
	$.ajax({
		url : "/SpringRestHibernateExample/updateSchedule",
		method : "PUT",
		contentType : "application/json",
		data : JSON.stringify(data),
		success : function() {
			Message("Thành công", "Bạn đã cập nhật dữ liệu thành công!",
					"success");
			$('#bootstrap-table').bootstrapTable('refresh');
			ResetInput();
		},
		error : function(error) {
			Message("Thất bại", "Đã có lỗi xảy ra!", "danger");
		}
	});
}

function DELETE(data) {
	$.ajax({
		url : "/SpringRestHibernateExample/deleteSchedule/" + data.id,
		method : "DELETE",
		contentType : "application/json",
		success : function() {
			Message("Thành công", "Bạn đã xoá dữ liệu này thành công!",
					"success");
			$('#bootstrap-table').bootstrapTable('refresh');
		},
		error : function() {
			Message("Thất bại", "Đã có lỗi xảy ra!", "danger");
		}
	});
}

function readURL(input) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();

		reader.onload = function(e) {
			document.getElementById('exercise-image').src = e.target.result;

		}

		reader.readAsDataURL(input.files[0]);
	}
}

function createCombobox(labelName, url, parentElem, valueField, displayField,
		field, comboboxNullVal) {
	$
			.ajax({
				url : "/SpringRestHibernateExample/" + url,
				method : "GET",
				contentType : "application/json",
				success : function(data) {
					var strs = [];
					var i = 0;
					strs[i++] = ([
							'<label class="col-sm-4 control-label"> ',
							labelName,
							'<star>*</star>',
							'</label>',
							'<div class="dropdown col-sm-7">',
							'<input class="form-control btn-default dropdown-toggle" onkeydown="return false" required="true" ',
							'type="text" data-toggle="dropdown"',
							'" placeholder="',
							comboboxNullVal,
							'" />',
							'<input class="form-control value-combobox" type="text" style="display: none;" require="true" aria-required="true" val="", name="',
							field,
							'"/><ul class="dropdown-menu dropdown-admin" role="menu"',
							'aria-labelledby="menu1">' ].join(''));

					$.each(data, function(key, val) {
						strs[i++] = ([
								'<li role="presentation"><a class="drop-item"',
								'role="menuitem" tabindex="-1">',
								val[displayField], '</a> <input ',
								'style="display: none" value="',
								val[valueField], '" /></li>', ].join(''));
					});

					strs[i++] = ([ '</ul>', '</div>' ].join(''));
					parentElem.append(strs.join(''));
					$
							.each(
									$('.drop-item'),
									function(key, val) {
										$(val)
												.click(
														function(e) {
															e.preventDefault();
															var button = $($(
																	val)
																	.parent()
																	.parent()
																	.parent()
																	.find(
																			'.form-control')[0]);
															var input = $($(val)
																	.parent()
																	.parent()
																	.parent()
																	.find(
																			'.value-combobox')[0]);
															button.val($(val)
																	.text());
															input
																	.val($(
																			$(
																					val)
																					.parent()
																					.find(
																							'input')[0])
																			.val());
														});
									});

				},
				error : function() {
					Message("Thất bại", "Đã có lỗi xảy ra!", "danger");
				}
			});
}
