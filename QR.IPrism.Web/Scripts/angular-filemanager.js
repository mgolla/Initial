! function (e, r, n) {
    "use strict";
    r.module("FileManagerApp", ["pascalprecht.translate", "ngCookies"]), n(e.document).on("shown.bs.modal", ".modal", function () {
        e.setTimeout(function () {
            n("[autofocus]", this).focus()
        }.bind(this), 100)
    }), n(e.document).on("click", function () {
        n("#context-menu").hide()
    }), n(e.document).on("contextmenu", ".main-navigation .table-files td:first-child, .iconset a.thumbnail", function (e) {
        n("#context-menu").hide().css({
            left: e.pageX,
            top: e.pageY
        }).show(), e.preventDefault()
    })
}(window, angular, jQuery),
function (e, r, n) {
    "use strict";
    r.module("FileManagerApp")
        .controller("FileManagerCtrl", ["$scope", "$translate", "$cookies", "fileManagerConfig", "item", "fileNavigator", "fileUploader", "fileService",
        function (r, t, a, i, o, s, l, fileService) {
            r.config = i, r.reverse = !1, r.predicate = ["model.type", "model.name"], r.order = function (e) {
                r.reverse = r.predicate[1] === e ? !r.reverse : !1, r.predicate[1] = e
            }, r.query = "", r.temp = new o, r.fileNavigator = new s, r.fileUploader = l, r.uploadFileList = [], r.viewTemplate = a.viewTemplate || "main-table.html", r.setTemplate = function (e) {
                r.viewTemplate = a.viewTemplate = e
            }, r.changeLanguage = function (e) {
                return e ? t.use(a.language = e) : void t.use(a.language || i.defaultLang)
            }, r.touch = function (e) {
                e = e instanceof o ? e : new o, e.revert(), r.temp = e
            }, r.smartClick = function (e) {
                var m = e.model;
                var filterPara = {
                    DocType: m.RowType,
                    FileCode: m.FileCode,
                    FileId: m.FileId,
                    DocumentId: m.DocumentId,
                    DocumentName: m.name,
                    isTab: false
                };


                return e.isFolder() ?
                  r.fileNavigator.folderClick(e) : fileService.openDocTypes(filterPara);

                //return e.isFolder() ?
                //    r.fileNavigator.folderClick(e) : e.isImage()
                //    ? r.config.previewImagesInModal
                //    ? r.openImagePreview(e) : e.download(!0) : e.isEditable()
                //    ? r.openEditItem(e) : void 0

            }, r.openImagePreview = function (e) {
                return e.inprocess = !0, r.modal("imagepreview").find("#imagepreview-target").attr("src", e.getUrl(!0)).unbind("load error").on("load error", function () {
                    e.inprocess = !1, r.$apply()
                }), r.touch(e)
            }, r.openEditItem = function (e) {
                return e.getContent(), r.modal("edit"), r.touch(e)
            }, r.modal = function (e, r) {
                return n("#" + e).modal(r ? "hide" : "show")
            }, r.isInThisPath = function (e) {
                var n = r.fileNavigator.currentPath.join("/");
                return -1 !== n.indexOf(e)
            }, r.edit = function (e) {
                e.edit().then(function () {
                    r.modal("edit", !0)
                })
            }, r.changePermissions = function (e) {
                e.changePermissions().then(function () {
                    r.modal("changepermissions", !0)
                })
            }, r.copy = function (e) {
                var n = e.tempModel.path.join() === e.model.path.join();
                return n && r.fileNavigator.fileNameExists(e.tempModel.name) ? (e.error = t.instant("error_invalid_filename"), !1) : void e.copy().then(function () {
                    r.fileNavigator.refresh(), r.modal("copy", !0)
                })
            }, r.compress = function (e) {
                e.compress().then(function () {
                    return r.fileNavigator.refresh(), r.config.compressAsync ? void (e.asyncSuccess = !0) : r.modal("compress", !0)
                }, function () {
                    e.asyncSuccess = !1
                })
            }, r.extract = function (e) {
                e.extract().then(function () {
                    return r.fileNavigator.refresh(), r.config.extractAsync ? void (e.asyncSuccess = !0) : r.modal("extract", !0)
                }, function () {
                    e.asyncSuccess = !1
                })
            }, r.remove = function (e) {
                e.remove().then(function () {
                    r.fileNavigator.refresh(), r.modal("delete", !0)
                })
            }, r.rename = function (e) {
                var n = e.tempModel.path.join() === e.model.path.join();
                return n && r.fileNavigator.fileNameExists(e.tempModel.name) ? (e.error = t.instant("error_invalid_filename"), !1) : void e.rename().then(function () {
                    r.fileNavigator.refresh(), r.modal("rename", !0)
                })
            }, r.createFolder = function (e) {
                var n = e.tempModel.name && e.tempModel.name.trim();
                return e.tempModel.type = "dir", e.tempModel.path = r.fileNavigator.currentPath, !n || r.fileNavigator.fileNameExists(n) ? (e.error = t.instant("error_invalid_filename"), !1) : void e.createFolder().then(function () {
                    r.fileNavigator.refresh(), r.modal("newfolder", !0)
                })
            }, r.uploadFiles = function () {
                r.fileUploader.upload(r.uploadFileList, r.fileNavigator.currentPath).then(function () {
                    r.fileNavigator.refresh(), r.modal("uploadfile", !0)
                }, function (e) {
                    var n = e.result && e.result.error || t.instant("error_uploading_files");
                    r.temp.error = n
                })
            }, r.getQueryParam = function (r) {
                var n;
                return e.location.search.substr(1).split("&").forEach(function (e) {
                    return r === e.split("=")[0] ? (n = e.split("=")[1], !1) : void 0
                }), n
            }, r.changeLanguage(r.getQueryParam("lang")), r.isWindows = "Windows" === r.getQueryParam("server"), r.fileNavigator.refresh()
        }])
}(window, angular, jQuery),
function (e, r) {
    "use strict";
    e.module("FileManagerApp").controller("ModalFileManagerCtrl", ["$scope", "$rootScope", "fileNavigator", function (e, n, t) {
        e.reverse = !1, e.predicate = ["model.type", "model.name"], e.order = function (r) {
            e.reverse = e.predicate[1] === r ? !e.reverse : !1, e.predicate[1] = r
        }, e.fileNavigator = new t, n.select = function (e, n) {
            n.tempModel.path = e.model.fullPath().split("/"), r("#selector").modal("hide")
        }, n.openNavigator = function (n) {
            e.fileNavigator.currentPath = n.model.path.slice(), e.fileNavigator.refresh(), r("#selector").modal("show")
        }
    }])
}(angular, jQuery),
function (e) {
    "use strict";
    e.module("FileManagerApp").service("chmod", function () {
        var e = function (e) {
            if (this.owner = this.getRwxObj(), this.group = this.getRwxObj(), this.others = this.getRwxObj(), e) {
                var r = isNaN(e) ? this.convertfromCode(e) : this.convertfromOctal(e);
                if (!r) throw new Error("Invalid chmod input data");
                this.owner = r.owner, this.group = r.group, this.others = r.others
            }
        };
        return e.prototype.toOctal = function (e, r) {
            var n = ["owner", "group", "others"],
                t = [];
            for (var a in n) {
                var i = n[a];
                t[a] = this[i].read && this.octalValues.read || 0, t[a] += this[i].write && this.octalValues.write || 0, t[a] += this[i].exec && this.octalValues.exec || 0
            }
            return (e || "") + t.join("") + (r || "")
        }, e.prototype.toCode = function (e, r) {
            var n = ["owner", "group", "others"],
                t = [];
            for (var a in n) {
                var i = n[a];
                t[a] = this[i].read && this.codeValues.read || "-", t[a] += this[i].write && this.codeValues.write || "-", t[a] += this[i].exec && this.codeValues.exec || "-"
            }
            return (e || "") + t.join("") + (r || "")
        }, e.prototype.getRwxObj = function () {
            return {
                read: !1,
                write: !1,
                exec: !1
            }
        }, e.prototype.octalValues = {
            read: 4,
            write: 2,
            exec: 1
        }, e.prototype.codeValues = {
            read: "r",
            write: "w",
            exec: "x"
        }, e.prototype.convertfromCode = function (e) {
            if (e = ("" + e).replace(/\s/g, ""), e = 10 === e.length ? e.substr(1) : e, /^[-rwxt]{9}$/.test(e)) {
                var r = [],
                    n = e.match(/.{1,3}/g);
                for (var t in n) {
                    var a = this.getRwxObj();
                    a.read = /r/.test(n[t]), a.write = /w/.test(n[t]), a.exec = /x|t/.test(n[t]), r.push(a)
                }
                return {
                    owner: r[0],
                    group: r[1],
                    others: r[2]
                }
            }
        }, e.prototype.convertfromOctal = function (e) {
            if (e = ("" + e).replace(/\s/g, ""), e = 4 === e.length ? e.substr(1) : e, /^[0-7]{3}$/.test(e)) {
                var r = [],
                    n = e.match(/.{1}/g);
                for (var t in n) {
                    var a = this.getRwxObj();
                    a.read = /[4567]/.test(n[t]), a.write = /[2367]/.test(n[t]), a.exec = /[1357]/.test(n[t]), r.push(a)
                }
                return {
                    owner: r[0],
                    group: r[1],
                    others: r[2]
                }
            }
        }, e
    })
}(angular),
function (e, r, n) {
    "use strict";
    r.module("FileManagerApp").factory("item", ["$http", "$q", "$translate", "fileManagerConfig", "chmod", function (t, a, i, o, s) {
        var l = function (e, n) {
            function t(e) {
                var r = (e || "").toString().split(/[- :]/);
                return new Date(r[0], r[1] - 1, r[2], r[3], r[4], r[5])
            }
            var a = {
                DocumentId: e && e.DocumentId || "",
                FileCode: e && e.FileCode || "",
                FileId: e && e.FileId || "",
                RowType: e && e.RowType || "",

                name: e && e.name || "",
                path: n || [],
                type: e && e.type || "file",
                size: e && parseInt(e.size || 0),
                date: t(e && e.date),
                perms: new s(e && e.rights),
                content: e && e.content || "",
                recursive: !1,
                sizeKb: function () {
                    return Math.round(this.size / 1024, 1)
                },
                fullPath: function () {
                    return ("/" + this.path.join("/") + "/" + this.name).replace(/\/\//, "/")
                }
            };
            this.error = "", this.inprocess = !1, this.model = r.copy(a), this.tempModel = r.copy(a)
        };
        return l.prototype.update = function () {
            r.extend(this.model, r.copy(this.tempModel))
        }, l.prototype.revert = function () {
            r.extend(this.tempModel, r.copy(this.model)), this.error = ""
        }, l.prototype.deferredHandler = function (e, r, n) {
            return e && "object" == typeof e || (this.error = "Bridge response error, please check the docs"), e.result && e.result.error && (this.error = e.result.error), !this.error && e.error && (this.error = e.error.message), !this.error && n && (this.error = n), this.error ? r.reject(e) : (this.update(), r.resolve(e))
        }, l.prototype.createFolder = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "addfolder",
                        path: e.tempModel.path.join("/"),
                        name: e.tempModel.name
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.createFolderUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_creating_folder"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.rename = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "rename",
                        path: e.model.fullPath(),
                        newPath: e.tempModel.fullPath()
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.renameUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_renaming"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.copy = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "copy",
                        path: e.model.fullPath(),
                        newPath: e.tempModel.fullPath()
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.copyUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_copying"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.compress = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "compress",
                        path: e.model.fullPath(),
                        destination: e.tempModel.fullPath()
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.compressUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_compressing"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.extract = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "extract",
                        path: e.model.fullPath(),
                        sourceFile: e.model.fullPath(),
                        destination: e.tempModel.fullPath()
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.extractUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_extracting"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.getUrl = function (e) {
            var r = this.model.fullPath(),
                t = {
                    mode: "download",
                    preview: e,
                    path: r
                };
            return r && [o.downloadFileUrl, n.param(t)].join("?")
        }, l.prototype.download = function (r) {
            "dir" !== this.model.type && e.open(this.getUrl(r), "_blank", "")
        }, l.prototype.getContent = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "editfile",
                        path: e.tempModel.fullPath()
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.getContentUrl, n).success(function (n) {
                e.tempModel.content = e.model.content = n.result, e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_getting_content"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.remove = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "delete",
                        path: e.tempModel.fullPath()
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.removeUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_deleting"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.edit = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "savefile",
                        content: e.tempModel.content,
                        path: e.tempModel.fullPath()
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.editUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_modifying"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.changePermissions = function () {
            var e = this,
                r = a.defer(),
                n = {
                    params: {
                        mode: "changepermissions",
                        path: e.tempModel.fullPath(),
                        perms: e.tempModel.perms.toOctal(),
                        permsCode: e.tempModel.perms.toCode(),
                        recursive: e.tempModel.recursive
                    }
                };
            return e.inprocess = !0, e.error = "", t.post(o.permissionsUrl, n).success(function (n) {
                e.deferredHandler(n, r)
            }).error(function (n) {
                e.deferredHandler(n, r, i.instant("error_changing_perms"))
            })["finally"](function () {
                e.inprocess = !1
            }), r.promise
        }, l.prototype.isFolder = function () {
            return "dir" === this.model.type
        }, l.prototype.isEditable = function () {
            return !this.isFolder() && o.isEditableFilePattern.test(this.model.name)
        }, l.prototype.isImage = function () {
            return o.isImageFilePattern.test(this.model.name)
        }, l.prototype.isCompressible = function () {
            return this.isFolder()
        }, l.prototype.isExtractable = function () {
            return !this.isFolder() && o.isExtractableFilePattern.test(this.model.name)
        }, l
    }])
}(window, angular, jQuery),
function (e) {
    "use strict";
    var r = e.module("FileManagerApp");
    r.directive("angularFilemanager", ["$parse", "fileManagerConfig", function (e, r) {
        return {
            restrict: "EA",
            templateUrl: r.tplPath + "/main.html"
        }
    }]), r.directive("ngFile", ["$parse", function (e) {
        return {
            restrict: "A",
            link: function (r, n, t) {
                var a = e(t.ngFile),
                    i = a.assign;
                n.bind("change", function () {
                    r.$apply(function () {
                        i(r, n[0].files)
                    })
                })
            }
        }
    }]), r.directive("ngRightClick", ["$parse", function (e) {
        return function (r, n, t) {
            var a = e(t.ngRightClick);
            n.bind("contextmenu", function (e) {
                r.$apply(function () {
                    e.preventDefault(), a(r, {
                        $event: e
                    })
                })
            })
        }
    }])
}(angular),
function (e) {
    "use strict";
    var r = e.module("FileManagerApp");
    r.filter("strLimit", ["$filter", function (e) {
        return function (r, n) {
            return r.length <= n ? r : e("limitTo")(r, n) + "..."
        }
    }]), r.filter("formatDate", ["$filter", function () {
        return function (e) {
            return e instanceof Date ? e.toISOString().substring(0, 19).replace("T", " ") : (e.toLocaleString || e.toString).apply(e)
        }
    }])
}(angular),
function (e) {
    "use strict";
    e.module("FileManagerApp").provider("fileManagerConfig", function () {
        var r = {
            appName: "https://github.com/joni2back/angular-filemanager",
            defaultLang: "en",
            listUrl: "bridges/php/handler.php",
            uploadUrl: "bridges/php/handler.php",
            renameUrl: "bridges/php/handler.php",
            copyUrl: "bridges/php/handler.php",
            removeUrl: "bridges/php/handler.php",
            editUrl: "bridges/php/handler.php",
            getContentUrl: "bridges/php/handler.php",
            createFolderUrl: "bridges/php/handler.php",
            downloadFileUrl: "bridges/php/handler.php",
            compressUrl: "bridges/php/handler.php",
            extractUrl: "bridges/php/handler.php",
            permissionsUrl: "bridges/php/handler.php",
            searchForm: !0,
            sidebar: !0,
            breadcrumb: !0,
            allowedActions: {
                upload: !0,
                rename: !0,
                copy: !0,
                edit: !0,
                changePermissions: !0,
                compress: !0,
                compressChooseName: !0,
                extract: !0,
                download: !0,
                preview: !0,
                remove: !0
            },
            previewImagesInModal: !0,
            enablePermissionsRecursive: !0,
            compressAsync: !0,
            extractAsync: !0,
            isEditableFilePattern: /\.(txt|html?|aspx?|ini|pl|py|md|css|js|log|htaccess|htpasswd|json|sql|xml|xslt?|sh|rb|as|bat|cmd|coffee|php[3-6]?|java|c|cbl|go|h|scala|vb)$/i,
            isImageFilePattern: /\.(jpe?g|gif|bmp|png|svg|tiff?)$/i,
            isExtractableFilePattern: /\.(gz|tar|rar|g?zip)$/i,
            tplPath: "src/templates"
        };
        return {
            $get: function () {
                return r
            },
            set: function (n) {
                e.extend(r, n)
            }
        }
    })
}(angular),
function (e) {
    "use strict";
    e.module("FileManagerApp").config(["$translateProvider", function (e) {
        e.translations("en", {
            filemanager: "File Manager",
            language: "Language",
            english: "English",
            spanish: "Spanish",
            portuguese: "Portuguese",
            french: "French",
            confirm: "Confirm",
            cancel: "Cancel",
            close: "Close",
            upload_file: "Upload file",
            files_will_uploaded_to: "Files will be uploaded to",
            uploading: "Uploading",
            permissions: "Permissions",
            select_destination_folder: "Select the destination folder",
            source: "Source",
            destination: "Destination",
            copy_file: "Copy file",
            sure_to_delete: "Are you sure to delete",
            change_name_move: "Change name / move",
            enter_new_name_for: "Enter new name for",
            extract_item: "Extract item",
            extraction_started: "Extraction started in a background process",
            compression_started: "Compression started in a background process",
            enter_folder_name_for_extraction: "Enter the folder name for the extraction of",
            enter_folder_name_for_compression: "Enter the folder name for the compression of",
            toggle_fullscreen: "Toggle fullscreen",
            edit_file: "Edit file",
            file_content: "File content",
            loading: "Loading",
            search: "Search",
            create_folder: "Create folder",
            create: "Create",
            folder_name: "Folder name",
            upload: "Upload",
            change_permissions: "Change permissions",
            change: "Change",
            details: "Details",
            icons: "Icons",
            list: "List",
            name: "Name",
            size: "Size",
            actions: "Actions",
            date: "Date",
            no_files_in_folder: "No files in this folder",
            no_folders_in_folder: "This folder not contains children folders",
            select_this: "Select this",
            go_back: "Go back",
            wait: "Wait",
            move: "Move",
            download: "Download",
            view_item: "View item",
            remove: "Delete",
            edit: "Edit",
            copy: "Copy",
            rename: "Rename",
            extract: "Extract",
            compress: "Compress",
            error_invalid_filename: "Invalid filename or already exists, specify another name",
            error_modifying: "An error occurred modifying the file",
            error_deleting: "An error occurred deleting the file or folder",
            error_renaming: "An error occurred renaming the file",
            error_copying: "An error occurred copying the file",
            error_compressing: "An error occurred compressing the file or folder",
            error_extracting: "An error occurred extracting the file",
            error_creating_folder: "An error occurred creating the folder",
            error_getting_content: "An error occurred getting the content of the file",
            error_changing_perms: "An error occurred changing the permissions of the file",
            error_uploading_files: "An error occurred uploading files",
            sure_to_start_compression_with: "Are you sure to compress",
            owner: "Owner",
            group: "Group",
            others: "Others",
            read: "Read",
            write: "Write",
            exec: "Exec",
            original: "Original",
            changes: "Changes",
            recursive: "Recursive",
            preview: "Item preview",
            open: "Open"
        }), e.translations("pt", {
            filemanager: "Gerenciador de arquivos",
            language: "Língua",
            english: "Inglês",
            spanish: "Espanhol",
            portuguese: "Portugues",
            french: "Francês",
            confirm: "Confirmar",
            cancel: "Cancelar",
            close: "Fechar",
            upload_file: "Carregar arquivo",
            files_will_uploaded_to: "Os arquivos serão enviados para",
            uploading: "Carregar",
            permissions: "Autorizações",
            select_destination_folder: "Selecione a pasta de destino",
            source: "Origem",
            destination: "Destino",
            copy_file: "Copiar arquivo",
            sure_to_delete: "Tem certeza de que deseja apagar",
            change_name_move: "Renomear / mudança",
            enter_new_name_for: "Digite o novo nome para",
            extract_item: "Extrair arquivo",
            extraction_started: "A extração começou em um processo em segundo plano",
            compression_started: "A compressão começou em um processo em segundo plano",
            enter_folder_name_for_extraction: "Digite o nome da pasta para a extração de",
            enter_folder_name_for_compression: "Digite o nome da pasta para Compressão",
            toggle_fullscreen: "Ativar/desativar tela cheia",
            edit_file: "Editar arquivo",
            file_content: "Conteúdo do arquivo",
            loading: "Carregando",
            search: "Localizar",
            create_folder: "Criar Pasta",
            create: "Criar",
            folder_name: "Nome da pasta",
            upload: "Fazer",
            change_permissions: "Alterar permissões",
            change: "Alterar",
            details: "Detalhes",
            icons: "Icones",
            list: "Lista",
            name: "Nome",
            size: "Tamanho",
            actions: "Ações",
            date: "Data",
            no_files_in_folder: "Não há arquivos nesta pasta",
            no_folders_in_folder: "Esta pasta não contém subpastas",
            select_this: "Selecione esta",
            go_back: "Voltar",
            wait: "Espere",
            move: "Mover",
            download: "Baixar",
            view_item: "Veja o arquivo",
            remove: "Excluir",
            edit: "Editar",
            copy: "Copiar",
            rename: "Renomear",
            extract: "Extrair",
            compress: "Comprimir",
            error_invalid_filename: "Nome do arquivo inválido ou nome de arquivo já existe, especifique outro nome",
            error_modifying: "Ocorreu um erro ao modificar o arquivo",
            error_deleting: "Ocorreu um erro ao excluir o arquivo ou pasta",
            error_renaming: "Ocorreu um erro ao mudar o nome do arquivo",
            error_copying: "Ocorreu um erro ao copiar o arquivo",
            error_compressing: "Ocorreu um erro ao comprimir o arquivo ou pasta",
            error_extracting: "Ocorreu um erro ao extrair o arquivo",
            error_creating_folder: "Ocorreu um erro ao criar a pasta",
            error_getting_content: "Ocorreu um erro ao obter o conteúdo do arquivo",
            error_changing_perms: "Ocorreu um erro ao alterar as permissões do arquivo",
            error_uploading_files: "Ocorreu um erro upload de arquivos",
            sure_to_start_compression_with: "Tem certeza que deseja comprimir",
            owner: "Proprietário",
            group: "Grupo",
            others: "Outros",
            read: "Leitura",
            write: "Escrita ",
            exec: "Execução",
            original: "Original",
            changes: "Mudanças",
            recursive: "Recursiva",
            preview: "Visualização",
            open: "Abrir"
        }), e.translations("es", {
            filemanager: "Administrador de archivos",
            language: "Idioma",
            english: "Ingles",
            spanish: "Español",
            portuguese: "Portugues",
            french: "Francés",
            confirm: "Confirmar",
            cancel: "Cancelar",
            close: "Cerrar",
            upload_file: "Subir archivo",
            files_will_uploaded_to: "Los archivos seran subidos a",
            uploading: "Subiendo",
            permissions: "Permisos",
            select_destination_folder: "Seleccione la carpeta de destino",
            source: "Origen",
            destination: "Destino",
            copy_file: "Copiar archivo",
            sure_to_delete: "Esta seguro que desea eliminar",
            change_name_move: "Renombrar / mover",
            enter_new_name_for: "Ingrese el nuevo nombre para",
            extract_item: "Extraer archivo",
            extraction_started: "La extraccion ha comenzado en un proceso de segundo plano",
            compression_started: "La compresion ha comenzado en un proceso de segundo plano",
            enter_folder_name_for_extraction: "Ingrese el nombre de la carpeta para la extraccion de",
            enter_folder_name_for_compression: "Ingrese el nombre de la carpeta para la compresion de",
            toggle_fullscreen: "Activar/Desactivar pantalla completa",
            edit_file: "Editar archivo",
            file_content: "Contenido del archivo",
            loading: "Cargando",
            search: "Buscar",
            create_folder: "Crear carpeta",
            create: "Crear",
            folder_name: "Nombre de la carpeta",
            upload: "Subir",
            change_permissions: "Cambiar permisos",
            change: "Cambiar",
            details: "Detalles",
            icons: "Iconos",
            list: "Lista",
            name: "Nombre",
            size: "Tamaño",
            actions: "Acciones",
            date: "Fecha",
            no_files_in_folder: "No hay archivos en esta carpeta",
            no_folders_in_folder: "Esta carpeta no contiene sub-carpetas",
            select_this: "Seleccionar esta",
            go_back: "Volver",
            wait: "Espere",
            move: "Mover",
            download: "Descargar",
            view_item: "Ver archivo",
            remove: "Eliminar",
            edit: "Editar",
            copy: "Copiar",
            rename: "Renombrar",
            extract: "Extraer",
            compress: "Comprimir",
            error_invalid_filename: "El nombre del archivo es invalido o ya existe",
            error_modifying: "Ocurrio un error al intentar modificar el archivo",
            error_deleting: "Ocurrio un error al intentar eliminar el archivo",
            error_renaming: "Ocurrio un error al intentar renombrar el archivo",
            error_copying: "Ocurrio un error al intentar copiar el archivo",
            error_compressing: "Ocurrio un error al intentar comprimir el archivo",
            error_extracting: "Ocurrio un error al intentar extraer el archivo",
            error_creating_folder: "Ocurrio un error al intentar crear la carpeta",
            error_getting_content: "Ocurrio un error al obtener el contenido del archivo",
            error_changing_perms: "Ocurrio un error al cambiar los permisos del archivo",
            error_uploading_files: "Ocurrio un error al subir archivos",
            sure_to_start_compression_with: "Esta seguro que desea comprimir",
            owner: "Propietario",
            group: "Grupo",
            others: "Otros",
            read: "Lectura",
            write: "Escritura",
            exec: "Ejecucion",
            original: "Original",
            changes: "Cambios",
            recursive: "Recursivo",
            preview: "Vista previa",
            open: "Abrir"
        }), e.translations("fr", {
            filemanager: "Gestionnaire de fichier",
            language: "Langue",
            english: "Anglais",
            spanish: "Espagnol",
            portuguese: "Portugais",
            french: "Français",
            confirm: "Confirmer",
            cancel: "Annuler",
            close: "Fermer",
            upload_file: "Uploader un fichier",
            files_will_uploaded_to: "Les fichiers seront uploadé dans",
            uploading: "Upload en cours",
            permissions: "Permissions",
            select_destination_folder: "Sélectionné le dossier de destination",
            source: "Source",
            destination: "Destination",
            copy_file: "Copier le fichier",
            sure_to_delete: "Êtes-vous sûr de vouloir supprimer",
            change_name_move: "Renommer / Déplacer",
            enter_new_name_for: "Entrer le nouveau nom pour",
            extract_item: "Extraires les éléments",
            extraction_started: "L'extraction a démarré en tâche de fond",
            compression_started: "La compression a démarré en tâche de fond",
            enter_folder_name_for_extraction: "Entrer le nom du dossier pour l'extraction de",
            enter_folder_name_for_compression: "Entrer le nom du dossier pour la compression de",
            toggle_fullscreen: "Basculer en plein écran",
            edit_file: "Éditer le fichier",
            file_content: "Contenu du fichier",
            loading: "Chargement en cours",
            search: "Recherche",
            create_folder: "Créer un dossier",
            create: "Créer",
            folder_name: "Nom du dossier",
            upload: "Upload",
            change_permissions: "Changer les permissions",
            change: "Changer",
            details: "Details",
            icons: "Icons",
            list: "Liste",
            name: "Nom",
            size: "Taille",
            actions: "Actions",
            date: "Date",
            no_files_in_folder: "Aucun fichier dans ce dossier",
            no_folders_in_folder: "Ce dossier ne contiens pas de dossier",
            select_this: "Sélectionner",
            go_back: "Retour",
            wait: "Patienter",
            move: "Déplacer",
            download: "Télécharger",
            view_item: "Voir l'élément",
            remove: "Supprimer",
            edit: "Éditer",
            copy: "Copier",
            rename: "Renommer",
            extract: "Extraire",
            compress: "Compresser",
            error_invalid_filename: "Nom de fichier invalide ou déjà existant, merci de spécifier un autre nom",
            error_modifying: "Une erreur est survenue pendant la modification du fichier",
            error_deleting: "Une erreur est survenue pendant la suppression du fichier ou du dossier",
            error_renaming: "Une erreur est survenue pendant le renommage du fichier",
            error_copying: "Une erreur est survenue pendant la copie du fichier",
            error_compressing: "Une erreur est survenue pendant la compression du fichier ou du dossier",
            error_extracting: "Une erreur est survenue pendant l'extraction du fichier",
            error_creating_folder: "Une erreur est survenue pendant la création du dossier",
            error_getting_content: "Une erreur est survenue pendant la récupération du contenu du fichier",
            error_changing_perms: "Une erreur est survenue pendant le changement des permissions du fichier",
            error_uploading_files: "Une erreur est survenue pendant l'upload des fichiers",
            sure_to_start_compression_with: "Êtes-vous sûre de vouloir compresser",
            owner: "Propriétaire",
            group: "Groupe",
            others: "Autres",
            read: "Lecture",
            write: "Écriture",
            exec: "Éxécution",
            original: "Original",
            changes: "Modifications",
            recursive: "Récursif",
            preview: "Aperçu",
            open: "Ouvrir"
        })
    }])
}(angular),
function (e) {
    "use strict";
    e.module("FileManagerApp").service("fileNavigator", ["$http", "$q", "fileManagerConfig", "item", "appSettings", function (e, r, n, t, appSettings) {
        e.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
        var a = function () {
            this.requesting = !1, this.fileList = [], this.currentPath = [], this.currentObjectPath = [], this.history = [], this.error = ""
        };
        return a.prototype.deferredHandler = function (e, r, n) {
            return e && "object" == typeof e || (this.error = "Bridge response error, please check the docs"), !this.error && e.result && e.result.error && (this.error = e.result.error), !this.error && e.error && (this.error = e.error.message), !this.error && n && (this.error = n), this.error ? r.reject(e) : r.resolve(e)
        }, a.prototype.list = function () {
            var t = this,
                a = r.defer(),
                i = t.currentPath.join("/"),
                o = {
                    params: {
                        mode: "list",
                        onlyFolders: !1,
                        path: "/" + i,
                        documentId: appSettings.DocumentId
                    }
                };

            return t.requesting = !0, t.fileList = [], t.error = "", e.post(n.listUrl, o).success(function (e) {
                
                if (appSettings.DocumentPath != "") {
                    //o.params.path = appSettings.DocumentPath;
                    appSettings.DocumentPath = "";
                    var dta = $.grep(e.result, function (v, i) {

                        return v.DocumentId == appSettings.DocumentId;
                    });
                    if (dta) {
                        setTimeout(function () { $('#doc-' + appSettings.DocumentId).click(); }, 100);
                        setTimeout(function () {
                            if (appSettings.SecondDocumentId && appSettings.SecondDocumentId != "") {
                                $('#doc-' + appSettings.SecondDocumentId).click();
                                appSettings.SecondDocumentId = "";
                            }                         

                        }, 150);
                    }


                }


                t.deferredHandler(e, a)
            }).error(function (e) {
                t.deferredHandler(e, a, "Unknown error listing, check the response")
            })["finally"](function () {
                t.requesting = !1
            }), a.promise
        }, a.prototype.refresh = function () {
            var e = this,
                r = e.currentPath.join("/");
            return e.list().then(function (n) {
                e.fileList = (n.result || []).map(function (r) {
                    return new t(r, e.currentPath)
                }), e.buildTree(r)
            })
        }, a.prototype.buildTree = function (e) {
            function r(e, n, t) {
                var a = t ? t + "/" + n.model.name : n.model.name;
                if (e.name.trim() && 0 !== t.trim().indexOf(e.name) && (e.nodes = []), e.name !== t)
                    for (var i in e.nodes) r(e.nodes[i], n, t);
                else {
                    for (var o in e.nodes)
                        if (e.nodes[o].name === a) return;
                    e.nodes.push({
                        item: n,
                        name: a,
                        nodes: []
                    })
                }
                e.nodes = e.nodes.sort(function (e, r) {
                    return e.name.toLowerCase() < r.name.toLowerCase() ? -1 : e.name.toLowerCase() === r.name.toLowerCase() ? 0 : 1
                })
            }

            function n(e, r) {
                r.push(e);
                for (var t in e.nodes) n(e.nodes[t], r)
            }

            function t(e, r) {
                return e.filter(function (e) {
                    return e.name === r
                })[0]
            }
            var a = [],
                i = {};
            !this.history.length && this.history.push({
                name: "",
                nodes: []
            }), n(this.history[0], a), i = t(a, e), i.nodes = [];
            for (var o in this.fileList) {
                var s = this.fileList[o];
                s.isFolder() && r(this.history[0], s, e)
            }
        }, a.prototype.folderClick = function (e) {
            appSettings.DocumentId = e.model.DocumentId;
            var folderdata = {
                path: e.model.fullPath().split("/").splice(1)[(e.model.fullPath().split("/").splice(1).length) - 1],
                documentId: e.model.DocumentId
            };
            this.currentObjectPath.push(folderdata);

            this.currentPath = [], e && e.isFolder() && (this.currentPath = e.model.fullPath().split("/").splice(1)), this.refresh()
        }, a.prototype.upDir = function () {
            this.currentPath[0] && (this.currentPath = this.currentPath.slice(0, -1), this.refresh())
        }, a.prototype.goTo = function (e) {
            var paraFolder = this.currentPath.slice(e, e + 1);
            if (paraFolder.length > 0) {
                var dta = $.grep(this.currentObjectPath, function (v, i) {

                    return v.path == paraFolder[0];
                });
                if (dta) {
                    appSettings.DocumentId = dta[0].documentId;
                }
            }
            this.currentPath = this.currentPath.slice(0, e + 1), this.refresh()
        }, a.prototype.fileNameExists = function (e) {
            for (var r in this.fileList)
                if (r = this.fileList[r], e.trim && r.model.name.trim() === e.trim()) return !0
        }, a.prototype.listHasFolders = function () {
            for (var e in this.fileList)
                if ("dir" === this.fileList[e].model.type) return !0
        }, a
    }])
}(angular),
function (e, r) {
    "use strict";
    r.module("FileManagerApp").service("fileUploader", ["$http", "$q", "fileManagerConfig", function (n, t, a) {
        function i(e, r, n) {
            return e && "object" == typeof e ? e.result && e.result.error ? r.reject(e) : e.error ? r.reject(e) : n ? r.reject(n) : void r.resolve(e) : r.reject("Bridge response error, please check the docs")
        }
        this.requesting = !1, this.upload = function (o, s) {
            if (!e.FormData) throw new Error("Unsupported browser version");
            var l = this,
                d = new e.FormData,
                c = t.defer();
            d.append("destination", "/" + s.join("/"));
            for (var m = 0; m < o.length; m++) {
                var p = o.item(m);
                p instanceof e.File && d.append("file-" + m, p)
            }
            return l.requesting = !0, n.post(a.uploadUrl, d, {
                transformRequest: r.identity,
                headers: {
                    "Content-Type": void 0
                }
            }).success(function (e) {
                i(e, c)
            }).error(function (e) {
                i(e, c, "Unknown error uploading files")
            })["finally"](function () {
                l.requesting = !1
            }), c.promise
        }
    }])
}(window, angular), angular.module("FileManagerApp").run(["$templateCache", function (e) {
    e.put("src/templates/current-folder-breadcrumb.html", '<ol class="breadcrumb mb0">\r\n    <li>\r\n        <a href="" ng-click="fileNavigator.goTo(-1)">\r\n            <i class="glyphicon glyphicon-folder-open mr2"></i>\r\n        </a>\r\n    </li>\r\n    <li ng-repeat="(key, dir) in fileNavigator.currentPath track by key" ng-class="{\'active\':$last}" class="animated fast fadeIn">\r\n        <a href="" ng-show="!$last" ng-click="fileNavigator.goTo(key)">\r\n            <i class="glyphicon glyphicon-folder-open mr2"></i> {{dir}}\r\n        </a>\r\n        <span ng-show="$last"><i class="glyphicon glyphicon-folder-open mr2"></i>  {{dir}}</span>\r\n    </li>\r\n    <li><button class="btn btn-primary btn-xs" ng-click="fileNavigator.upDir()">&crarr;</button></li>\r\n</ol>'),
    e.put("src/templates/item-context-menu.html", '<div id="context-menu" class="dropdown clearfix animated fast fadeIn">\r\n    <ul class="dropdown-menu dropdown-right-click" role="menu" aria-labelledby="dropdownMenu" style="">\r\n        \r\n        <li ng-show="temp.isFolder()">\r\n            <a href="" tabindex="-1" ng-click="smartClick(temp)">\r\n                <i class="glyphicon glyphicon-folder-open"></i> {{\'open\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.download && !temp.isFolder()">\r\n            <a href="" tabindex="-1" ng-click="temp.download()">\r\n                <i class="glyphicon glyphicon-download"></i> {{\'download\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.preview && temp.isImage()">\r\n            <a href="" tabindex="-1" ng-click="openImagePreview(temp)">\r\n                <i class="glyphicon glyphicon-picture"></i> {{\'view_item\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li class="divider"></li>\r\n\r\n        <li ng-show="config.allowedActions.rename">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#rename">\r\n                <i class="glyphicon glyphicon-edit"></i> {{\'rename\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.copy && !temp.isFolder()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#copy">\r\n                <i class="glyphicon glyphicon-log-out"></i> {{\'copy\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.edit && temp.isEditable()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#edit" ng-click="temp.getContent();">\r\n                <i class="glyphicon glyphicon-pencil"></i> {{\'edit\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.changePermissions">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#changepermissions">\r\n                <i class="glyphicon glyphicon-lock"></i> {{\'permissions\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.compress && temp.isCompressible()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#compress">\r\n                <i class="glyphicon glyphicon-compressed"></i> {{\'compress\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.extract && temp.isExtractable()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#extract" ng-click="temp.tempModel.name=\'\'">\r\n                <i class="glyphicon glyphicon-export"></i> {{\'extract\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li class="divider"></li>\r\n        \r\n        <li ng-show="config.allowedActions.remove">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#delete">\r\n                <i class="glyphicon glyphicon-trash"></i> {{\'remove\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n    </ul>\r\n</div>'),
        e.put("src/templates/item-toolbar.html", '<div ng-show="!item.inprocess">\r\n    <button class="hidden btn btn-sm btn-default" data-toggle="modal" data-target="#rename" ng-show="config.allowedActions.rename" ng-click="touch(item)" title="{{\'rename\' | translate}}">\r\n        <i class="glyphicon glyphicon-edit"></i>\r\n    </button>\r\n    <button class="hidden btn btn-sm btn-default" data-toggle="modal" data-target="#copy" ng-show="config.allowedActions.copy && !item.isFolder()" ng-click="touch(item)" title="{{\'copy\' | translate}}">\r\n        <i class="glyphicon glyphicon-log-out"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#edit" ng-show="config.allowedActions.edit && item.isEditable()" ng-click="item.getContent(); touch(item)" title="{{\'edit\' | translate}}">\r\n        <i class="glyphicon glyphicon-pencil"></i>\r\n    </button>\r\n    <button class="hidden btn btn-sm btn-default" data-toggle="modal" data-target="#changepermissions" ng-show="config.allowedActions.changePermissions" ng-click="touch(item)" title="{{\'permissions\' | translate}}">\r\n        <i class="glyphicon glyphicon-lock"></i>\r\n    </button>\r\n    <button class="hidden btn btn-sm btn-default" data-toggle="modal" data-target="#compress" ng-show="config.allowedActions.compress && item.isCompressible()" ng-click="touch(item)" title="{{\'compress\' | translate}}">\r\n        <i class="glyphicon glyphicon-compressed"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#extract" ng-show="config.allowedActions.extract && item.isExtractable()" ng-click="touch(item); item.tempModel.name=\'\'" title="{{\'extract\' | translate}}">\r\n        <i class="glyphicon glyphicon-export"></i>\r\n    </button>\r\n    <button class="hidden btn btn-sm btn-default" ng-show="config.allowedActions.download && !item.isFolder()" ng-click="item.download()" title="{{\'download\' | translate}}">\r\n        <i class="glyphicon glyphicon-cloud-download"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" ng-show="config.allowedActions.preview && item.isImage()" ng-click="openImagePreview(item)" title="{{\'view_item\' | translate}}">\r\n        <i class="glyphicon glyphicon-picture"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-danger" data-toggle="modal" data-target="#delete" ng-show="config.allowedActions.remove" ng-click="touch(item)" title="{{\'remove\' | translate}}">\r\n        <i class="glyphicon glyphicon-trash"></i>\r\n    </button>\r\n</div>\r\n<div ng-show="item.inprocess">\r\n    <button class="btn btn-sm" style="visibility: hidden">&nbsp;</button><span class="label label-warning">{{"wait" | translate}} ...</span>\r\n</div>'),
    e.put("src/templates/main-icons.html", '<div class="iconset clearfix">\r\n    <div class="col-120" ng-repeat="item in fileNavigator.fileList | filter: query | orderBy: orderProp" ng-show="!fileNavigator.requesting && !fileNavigator.error">\r\n        <a id="doc-{{item.model.DocumentId}}" href="" class="thumbnail text-center" ng-click="smartClick(item)"  title="{{item.model.name}} ({{item.model.sizeKb()}}kb)">\r\n            <div class="item-icon">\r\n                <i class="glyphicon glyphicon-folder-open" ng-show="item.model.type === \'dir\'"></i>\r\n                <i class="glyphicon glyphicon-file" ng-show="item.model.type === \'file\'"></i>\r\n            </div>\r\n            {{item.model.name | strLimit : 11 }}\r\n        </a>\r\n    </div>\r\n\r\n    <div ng-show="fileNavigator.requesting">\r\n        <div ng-include="config.tplPath + \'/spinner.html\'"></div>\r\n    </div>\r\n\r\n    <div class="alert alert-warning" ng-show="!fileNavigator.requesting && fileNavigator.fileList.length < 1 && !fileNavigator.error">\r\n        {{"no_files_in_folder" | translate}}...\r\n    </div>\r\n    \r\n    <div class="alert alert-danger" ng-show="!fileNavigator.requesting && fileNavigator.error">\r\n        {{ fileNavigator.error }}\r\n    </div>\r\n</div>')
    , e.put("src/templates/main-table-modal.html", '<table class="table table-condensed table-modal-condensed mb0">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                <a href="" ng-click="order(\'model.name\')">\r\n                    {{"name" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.name\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="text-right"></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody class="file-item">\r\n        <tr ng-show="fileNavigator.requesting">\r\n            <td colspan="2">\r\n                <div ng-include="config.tplPath + \'/spinner.html\'"></div>\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && !fileNavigator.listHasFolders() && !fileNavigator.error">\r\n            <td colspan="2">\r\n                {{"no_folders_in_folder" | translate}}...\r\n            </td>\r\n            <td class="text-right">\r\n                <button class="btn btn-sm btn-default" ng-click="fileNavigator.upDir()">{{"go_back" | translate}}</button>\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && fileNavigator.error">\r\n            <td colspan="2">\r\n                {{ fileNavigator.error }}\r\n            </td>\r\n        </tr>\r\n        <tr ng-repeat="item in fileNavigator.fileList | orderBy:predicate:reverse" ng-show="!fileNavigator.requesting && item.model.type === \'dir\'">\r\n            <td>\r\n                <a href="" ng-click="fileNavigator.folderClick(item)" title="{{item.model.name}} ({{item.model.sizeKb()}}kb)">\r\n                    <i class="glyphicon glyphicon-folder-close"></i>\r\n                    {{item.model.name | strLimit : 32}}\r\n                </a>\r\n            </td>\r\n            <td class="text-right">\r\n                <button class="btn btn-sm btn-default" ng-click="select(item, temp)">\r\n                    <i class="glyphicon glyphicon-hand-up"></i> {{"select_this" | translate}}\r\n                </button>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>'),
    e.put("src/templates/main-table.html", '<table class="table mb0 table-files">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                <a href="" ng-click="order(\'model.name\')">\r\n                    {{"name" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.name\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="hidden hidden-xs">\r\n                <a href="" ng-click="order(\'model.size\')">\r\n                    {{"size" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.size\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="hidden hidden-sm hidden-xs">\r\n                <a href="" ng-click="order(\'model.date\')">\r\n                    {{"date" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.date\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="hidden hidden-sm hidden-xs">\r\n                <a href="" ng-click="order(\'model.permissions\')">\r\n                    {{"permissions" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.permissions\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="text-right"></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody class="file-item">\r\n        <tr ng-show="fileNavigator.requesting">\r\n            <td colspan="5">\r\n                <div ng-include="config.tplPath + \'/spinner.html\'"></div>\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && fileNavigator.fileList.length < 1 && !fileNavigator.error">\r\n            <td colspan="5">\r\n                {{"no_files_in_folder" | translate}}...\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && fileNavigator.error">\r\n            <td colspan="5">\r\n                {{ fileNavigator.error }}\r\n            </td>\r\n        </tr>\r\n        <tr ng-repeat="item in fileNavigator.fileList | filter: {model:{name: query}} | orderBy:predicate:reverse" ng-show="!fileNavigator.requesting">\r\n            <td ng-right-click="touch(item)">\r\n                <a id="doc-{{item.model.DocumentId}}" href="" ng-click="smartClick(item)" title="{{item.model.name}} ({{item.model.sizeKb()}}kb)">\r\n                    <i class="glyphicon glyphicon-folder-close" ng-show="item.model.type === \'dir\'"></i>\r\n                    <i class="glyphicon glyphicon-file" ng-show="item.model.type === \'file\'"></i>\r\n                    {{item.model.name | strLimit : 64}}\r\n                </a>\r\n            </td>\r\n            <td class="hidden hidden-xs">\r\n                {{item.model.sizeKb()}}kb\r\n            </td>\r\n            <td class="hidden hidden-sm hidden-xs">\r\n                {{item.model.date | formatDate }}\r\n            </td>\r\n            <td class="hidden hidden-sm hidden-xs">\r\n                {{item.model.perms.toCode(item.model.type === \'dir\'?\'d\':\'-\')}}\r\n            </td>\r\n            <td class="text-right">\r\n                <div ng-include="config.tplPath + \'/item-toolbar.html\'"></div>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>'),
    e.put("src/templates/main.html", '<div ng-controller="FileManagerCtrl">\r\n    <div ng-include="config.tplPath + \'/navbar.html\'"></div>\r\n\r\n    <div class="file_mgr">\r\n        <div class="row">\r\n\r\n           <div class="col-lg-2 col-sm-12 col-md-2 sidebar  file-tree animated slow fadeIn" ng-include="config.tplPath + \'/sidebar.html\'" ng-show="config.sidebar && fileNavigator.history[0]"></div>\r\n            <div class="main" ng-class="config.sidebar && fileNavigator.history[0] && \' file_mgr_cont col-lg-10 col-sm-12  col-md-10\'">\r\n                <div  class="breadcrumb" ng-include="config.tplPath + \'/current-folder-breadcrumb.html\'" ng-show="config.breadcrumb"></div>\r\n                <div ng-include="config.tplPath + \'/\' + viewTemplate" class="main-navigation clearfix"></div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n    <div ng-include="config.tplPath + \'/modals.html\'"></div>\r\n    <div ng-include="config.tplPath + \'/item-context-menu.html\'"></div>\r\n</div>'),
    e.put("src/templates/modals.html", '<div class="modal animated fadeIn" id="imagepreview">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n    <form ng-submit="remove(temp)">\r\n      <div class="modal-header">\r\n        <button type="button" class="close" data-dismiss="modal">\r\n            <span aria-hidden="true">&times;</span>\r\n            <span class="sr-only">{{"close" | translate}}</span>\r\n        </button>\r\n        <h4 class="modal-title">{{"preview" | translate}}</h4>\r\n      </div>\r\n      <div class="modal-body" ng-show="temp.getUrl().length > 10">\r\n        <div class="text-center">\r\n          <img id="imagepreview-target" class="preview" alt="{{temp.model.name}}" ng-class="{\'loading\': temp.inprocess}">\r\n          <span class="label label-warning" ng-show="temp.inprocess">{{\'loading\' | translate}} ...</span>\r\n        </div>\r\n        <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n      </div>\r\n      <div class="modal-footer">\r\n        <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n      </div>\r\n      </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="delete">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n    <form ng-submit="remove(temp)">\r\n      <div class="modal-header">\r\n        <button type="button" class="close" data-dismiss="modal">\r\n            <span aria-hidden="true">&times;</span>\r\n            <span class="sr-only">{{"close" | translate}}</span>\r\n        </button>\r\n        <h4 class="modal-title">{{"confirm" | translate}}</h4>\r\n      </div>\r\n      <div class="modal-body">\r\n        {{\'sure_to_delete\' | translate}} <b>{{temp.model.name}}</b> ?\r\n        <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n      </div>\r\n      <div class="modal-footer">\r\n        <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n        <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess" autofocus="autofocus">{{"remove" | translate}}</button>\r\n      </div>\r\n      </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="rename">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="rename(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'change_name_move\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{\'enter_new_name_for\' | translate}} <b>{{temp.model.name}}</b></label>\r\n              <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n\r\n              <div ng-include data-src="\'path-selector\'" class="clearfix"></div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="hidden btn btn-primary" ng-disabled="temp.inprocess">{{\'rename\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="copy">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="copy(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'copy_file\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{\'enter_new_name_for\' | translate}} <b>{{temp.model.name}}</b></label>\r\n              <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n\r\n              <div ng-include data-src="\'path-selector\'" class="clearfix"></div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">Copy</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="compress">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="compress(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'compress\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <div class="label label-success error-msg">{{\'compression_started\' | translate}}</div>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <div ng-hide="config.allowedActions.compressChooseName">\r\n                    {{\'sure_to_start_compression_with\' | translate}} <b>{{temp.model.name}}</b> ?\r\n                  </div>\r\n                  <div ng-show="config.allowedActions.compressChooseName">\r\n                    <label class="radio">{{\'enter_folder_name_for_compression\' | translate}} <b>{{fileNavigator.currentPath.join(\'/\')}}/{{temp.model.name}}</b></label>\r\n                    <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n                  </div>\r\n              </div>\r\n\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n                  <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">{{\'compress\' | translate}}</button>\r\n              </div>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="extract" ng-init="temp.emptyName()">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="extract(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'extract_item\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <div class="label label-success error-msg">{{\'extraction_started\' | translate}}</div>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <label class="radio">{{\'enter_folder_name_for_extraction\' | translate}} <b>{{temp.model.name}}</b></label>\r\n                  <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n                  <div ng-include data-src="\'path-selector\'" class="clearfix"></div>\r\n              </div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n                  <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">{{\'extract\' | translate}}</button>\r\n              </div>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="edit" ng-class="{\'modal-fullscreen\': fullscreen}">\r\n  <div class="modal-dialog modal-lg">\r\n    <div class="modal-content">\r\n        <form ng-submit="edit(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <button type="button" class="close mr5" ng-click="fullscreen=!fullscreen">\r\n                  <span>&loz;</span>\r\n                  <span class="sr-only">{{\'toggle_fullscreen\' | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'edit_file\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n                <label class="radio">{{\'file_content\' | translate}}</label>\r\n                <span class="label label-warning" ng-show="temp.inprocess">{{\'loading\' | translate}} ...</span>\r\n                <textarea class="form-control code" ng-model="temp.tempModel.content" ng-show="!temp.inprocess" autofocus="autofocus"></textarea>\r\n                <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{\'close\' | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-show="config.allowedActions.edit" ng-disabled="temp.inprocess">{{\'edit\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="newfolder">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="createFolder(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'create_folder\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{\'folder_name\' | translate}}</label>\r\n              <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">{{\'create\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="uploadfile">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="uploadFiles()">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{"upload_file" | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{"files_will_uploaded_to" | translate}} <b>{{fileNavigator.currentPath.join(\'/\')}}</b></label>\r\n              <input type="file" class="form-control" ng-file="$parent.uploadFileList" autofocus="autofocus" multiple="multiple"/>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <div ng-show="!fileUploader.requesting">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal">{{"cancel" | translate}}</button>\r\n                  <button type="submit" class="btn btn-primary" ng-disabled="!uploadFileList.length || fileUploader.requesting">{{\'upload\' | translate}}</button>\r\n              </div>\r\n              <div ng-show="fileUploader.requesting">\r\n                  <span class="label label-warning">{{"uploading" | translate}} ...</span>\r\n              </div>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="changepermissions">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="changePermissions(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'change_permissions\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <table class="table mb0">\r\n                  <thead>\r\n                      <tr>\r\n                          <th>{{\'permissions\' | translate}}</th>\r\n                          <th class="col-xs-1 text-center">{{\'exec\' | translate}}</th>\r\n                          <th class="col-xs-1 text-center">{{\'read\' | translate}}</th>\r\n                          <th class="col-xs-1 text-center">{{\'write\' | translate}}</th>\r\n                      </tr>\r\n                  </thead>\r\n                  <tbody>\r\n                      <tr ng-repeat="(permTypeKey, permTypeValue) in temp.tempModel.perms">\r\n                          <td>{{permTypeKey | translate}}</td>\r\n                          <td ng-repeat="(permKey, permValue) in permTypeValue" class="col-xs-1 text-center" ng-click="main()">\r\n                              <label class="col-xs-12">\r\n                                <input type="checkbox" ng-model="temp.tempModel.perms[permTypeKey][permKey]">\r\n                              </label>\r\n                          </td>\r\n                      </tr>\r\n                </tbody>\r\n              </table>\r\n              <div class="checkbox" ng-show="config.enablePermissionsRecursive && temp.model.type === \'dir\'">\r\n                <label>\r\n                  <input type="checkbox" ng-model="temp.tempModel.recursive"> {{\'recursive\' | translate}}\r\n                </label>\r\n              </div>\r\n              <div class="clearfix mt10">\r\n                  <span class="label label-primary pull-left">\r\n                    {{\'original\' | translate}}: {{temp.model.perms.toCode(temp.model.type === \'dir\'?\'d\':\'-\')}} ({{temp.model.perms.toOctal()}})\r\n                  </span>\r\n                  <span class="label label-primary pull-right">\r\n                    {{\'changes\' | translate}}: {{temp.tempModel.perms.toCode(temp.model.type === \'dir\'?\'d\':\'-\')}} ({{temp.tempModel.perms.toOctal()}})\r\n                  </span>\r\n              </div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-disabled="">{{\'change\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="selector" ng-controller="ModalFileManagerCtrl">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n      <div class="modal-header">\r\n        <button type="button" class="close" data-dismiss="modal">\r\n            <span aria-hidden="true">&times;</span>\r\n            <span class="sr-only">{{"close" | translate}}</span>\r\n        </button>\r\n        <h4 class="modal-title">{{"select_destination_folder" | translate}}</h4>\r\n      </div>\r\n      <div class="modal-body">\r\n        <div>\r\n            <div ng-include="config.tplPath + \'/current-folder-breadcrumb.html\'"></div>\r\n            <div ng-include="config.tplPath + \'/main-table-modal.html\'"></div>\r\n        </div>\r\n      </div>\r\n      <div class="modal-footer">\r\n        <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<script type="text/ng-template" id="path-selector">\r\n  <div class="panel panel-primary mt10 mb0">\r\n    <div class="panel-body">\r\n        <div class="detail-sources">\r\n          <code class="mr5"><b>{{"source" | translate}}:</b> {{temp.model.fullPath()}}</code>\r\n        </div>\r\n        <div class="detail-sources">\r\n          <code class="mr5"><b>{{"destination" | translate}}:</b>{{temp.tempModel.fullPath()}}</code>\r\n          <a href="" class="label label-primary" ng-click="openNavigator(temp)">{{\'change\' | translate}}</a>\r\n        </div>\r\n    </div>\r\n  </div>\r\n</script>\r\n<script type="text/ng-template" id="error-bar">\r\n    <div class="label label-danger error-msg pull-left animated fadeIn" ng-show="temp.error">\r\n      <i class="glyphicon glyphicon-remove-circle"></i>\r\n      <span>{{temp.error}}</span>\r\n    </div>\r\n</script>'),
    // e.put("src/templates/navbar.html", '<nav class="navbar navbar-inverse navbar-fixed-top">\r\n  <div class="container-fluid">\r\n    <div class="navbar-header">\r\n      <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">\r\n        <span class="sr-only">Toggle</span>\r\n        <span class="icon-bar"></span>\r\n        <span class="icon-bar"></span>\r\n        <span class="icon-bar"></span>\r\n      </button>\r\n      <a class="navbar-brand hidden-xs" href="" ng-click="fileNavigator.goTo(-1)">{{config.appName}}</a>\r\n    </div>\r\n    <div id="navbar" class="navbar-collapse collapse">\r\n      <div class="navbar-form navbar-right">\r\n        <input type="text" class="form-control input-sm" ng-show="config.searchForm" placeholder="{{\'search\' | translate}}..." ng-model="$parent.query">\r\n        <button class="btn btn-default btn-sm" data-toggle="modal" data-target="#newfolder" ng-click="touch()">\r\n            <i class="glyphicon glyphicon-plus"></i> {{"create_folder" | translate}}\r\n        </button>\r\n        <button class="btn btn-default btn-sm" data-toggle="modal" data-target="#uploadfile" ng-show="config.allowedActions.upload" ng-click="touch()">\r\n            <i class="glyphicon glyphicon-upload"></i> {{"upload_file" | translate}}\r\n        </button>\r\n\r\n        <button class="btn btn-default btn-sm dropdown-toggle" type="button" id="dropDownMenuLang" data-toggle="dropdown" aria-expanded="true">\r\n            <i class="glyphicon glyphicon-globe"></i> {{"language" | translate}} <span class="caret"></span>\r\n        </button>\r\n        <ul class="dropdown-menu" role="menu" aria-labelledby="dropDownMenuLang">\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'en\')">{{"english" | translate}}</a></li>\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'es\')">{{"spanish" | translate}}</a></li>\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'pt\')">{{"portuguese" | translate}}</a></li>\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'fr\')">{{"french" | translate}}</a></li>\r\n        </ul>\r\n\r\n        <button class="btn btn-default btn-sm" ng-click="$parent.setTemplate(\'main-icons.html\')" ng-show="$parent.viewTemplate !== \'main-icons.html\'" title="{{\'icons\' | translate}}">\r\n            <i class="glyphicon glyphicon-th-large"></i>\r\n        </button>\r\n        <button class="btn btn-default btn-sm" ng-click="$parent.setTemplate(\'main-table.html\')" ng-show="$parent.viewTemplate !== \'main-table.html\'" title="{{\'list\' | translate}}">\r\n            <i class="glyphicon glyphicon-th-list"></i>\r\n        </button>\r\n\r\n      </div>\r\n    </div>\r\n  </div>\r\n</nav>'),
        e.put("src/templates/sidebar.html", '<h1>File Explorer </h1><ul class="nav nav-sidebar file-tree-root">\r\n    <li ng-repeat="item in fileNavigator.history" ng-include="\'folder-branch-item\'" ng-class="{\'active\': item.name == fileNavigator.currentPath.join(\'/\')}"></li>\r\n</ul>\r\n\r\n<script type="text/ng-template" id="folder-branch-item">\r\n    <a href="" ng-click="fileNavigator.folderClick(item.item)" class="animated fast fadeInDown">\r\n        <i class="glyphicon glyphicon-folder-close mr2" ng-hide="isInThisPath(item.name)"></i>\r\n        <i class="glyphicon glyphicon-folder-open mr2"  ng-show="isInThisPath(item.name)"></i>\r\n        {{ (item.name.split(\'/\').pop() || \'/\') | strLimit : 24 }}\r\n    </a>\r\n    <ul class="nav nav-sidebar">\r\n        <li ng-repeat="item in item.nodes" ng-include="\'folder-branch-item\'" ng-class="{\'active\': item.name == fileNavigator.currentPath.join(\'/\')}"></li>\r\n    </ul>\r\n</script>'), e.put("src/templates/spinner.html", '<div class="spinner-wrapper col-xs-12">\r\n    <svg class="spinner-container" style="width:65px;height:65px" viewBox="0 0 44 44">\r\n        <circle class="path" cx="22" cy="22" r="20" fill="none" stroke-width="4"></circle>\r\n    </svg>\r\n</div>')
}]);



//}(window, angular), angular.module("FileManagerApp").run(["$templateCache", function (e) {
//    e.put("src/templates/current-folder-breadcrumb.html", '<ol class="breadcrumb mb0">\r\n    <li>\r\n        <a href="" ng-click="fileNavigator.goTo(-1)">\r\n            <i class="glyphicon glyphicon-folder-open mr2"></i>\r\n        </a>\r\n    </li>\r\n    <li ng-repeat="(key, dir) in fileNavigator.currentPath track by key" ng-class="{\'active\':$last}" class="animated fast fadeIn">\r\n        <a href="" ng-show="!$last" ng-click="fileNavigator.goTo(key)">\r\n            <i class="glyphicon glyphicon-folder-open mr2"></i> {{dir}}\r\n        </a>\r\n        <span ng-show="$last"><i class="glyphicon glyphicon-folder-open mr2"></i>  {{dir}}</span>\r\n    </li>\r\n    <li><button class="btn btn-primary btn-xs" ng-click="fileNavigator.upDir()">&crarr;</button></li>\r\n</ol>'),
//    e.put("src/templates/item-context-menu.html", '<div id="context-menu" class="dropdown clearfix animated fast fadeIn">\r\n    <ul class="dropdown-menu dropdown-right-click" role="menu" aria-labelledby="dropdownMenu" style="">\r\n        \r\n        <li ng-show="temp.isFolder()">\r\n            <a href="" tabindex="-1" ng-click="smartClick(temp)">\r\n                <i class="glyphicon glyphicon-folder-open"></i> {{\'open\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.download && !temp.isFolder()">\r\n            <a href="" tabindex="-1" ng-click="temp.download()">\r\n                <i class="glyphicon glyphicon-download"></i> {{\'download\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.preview && temp.isImage()">\r\n            <a href="" tabindex="-1" ng-click="openImagePreview(temp)">\r\n                <i class="glyphicon glyphicon-picture"></i> {{\'view_item\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li class="divider"></li>\r\n\r\n        <li ng-show="config.allowedActions.rename">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#rename">\r\n                <i class="glyphicon glyphicon-edit"></i> {{\'rename\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.copy && !temp.isFolder()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#copy">\r\n                <i class="glyphicon glyphicon-log-out"></i> {{\'copy\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.edit && temp.isEditable()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#edit" ng-click="temp.getContent();">\r\n                <i class="glyphicon glyphicon-pencil"></i> {{\'edit\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.changePermissions">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#changepermissions">\r\n                <i class="glyphicon glyphicon-lock"></i> {{\'permissions\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.compress && temp.isCompressible()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#compress">\r\n                <i class="glyphicon glyphicon-compressed"></i> {{\'compress\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li ng-show="config.allowedActions.extract && temp.isExtractable()">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#extract" ng-click="temp.tempModel.name=\'\'">\r\n                <i class="glyphicon glyphicon-export"></i> {{\'extract\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n        <li class="divider"></li>\r\n        \r\n        <li ng-show="config.allowedActions.remove">\r\n            <a href="" tabindex="-1" data-toggle="modal" data-target="#delete">\r\n                <i class="glyphicon glyphicon-trash"></i> {{\'remove\' | translate}}\r\n            </a>\r\n        </li>\r\n\r\n    </ul>\r\n</div>'),
//        e.put("src/templates/item-toolbar.html", '<div ng-show="!item.inprocess">\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#rename" ng-show="config.allowedActions.rename" ng-click="touch(item)" title="{{\'rename\' | translate}}">\r\n        <i class="glyphicon glyphicon-edit"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#copy" ng-show="config.allowedActions.copy && !item.isFolder()" ng-click="touch(item)" title="{{\'copy\' | translate}}">\r\n        <i class="glyphicon glyphicon-log-out"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#edit" ng-show="config.allowedActions.edit && item.isEditable()" ng-click="item.getContent(); touch(item)" title="{{\'edit\' | translate}}">\r\n        <i class="glyphicon glyphicon-pencil"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#changepermissions" ng-show="config.allowedActions.changePermissions" ng-click="touch(item)" title="{{\'permissions\' | translate}}">\r\n        <i class="glyphicon glyphicon-lock"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#compress" ng-show="config.allowedActions.compress && item.isCompressible()" ng-click="touch(item)" title="{{\'compress\' | translate}}">\r\n        <i class="glyphicon glyphicon-compressed"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" data-toggle="modal" data-target="#extract" ng-show="config.allowedActions.extract && item.isExtractable()" ng-click="touch(item); item.tempModel.name=\'\'" title="{{\'extract\' | translate}}">\r\n        <i class="glyphicon glyphicon-export"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" ng-show="config.allowedActions.download && !item.isFolder()" ng-click="item.download()" title="{{\'download\' | translate}}">\r\n        <i class="glyphicon glyphicon-cloud-download"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-default" ng-show="config.allowedActions.preview && item.isImage()" ng-click="openImagePreview(item)" title="{{\'view_item\' | translate}}">\r\n        <i class="glyphicon glyphicon-picture"></i>\r\n    </button>\r\n    <button class="btn btn-sm btn-danger" data-toggle="modal" data-target="#delete" ng-show="config.allowedActions.remove" ng-click="touch(item)" title="{{\'remove\' | translate}}">\r\n        <i class="glyphicon glyphicon-trash"></i>\r\n    </button>\r\n</div>\r\n<div ng-show="item.inprocess">\r\n    <button class="btn btn-sm" style="visibility: hidden">&nbsp;</button><span class="label label-warning">{{"wait" | translate}} ...</span>\r\n</div>'),
//    e.put("src/templates/main-icons.html", '<div class="iconset clearfix">\r\n    <div class="col-120" ng-repeat="item in fileNavigator.fileList | filter: query | orderBy: orderProp" ng-show="!fileNavigator.requesting && !fileNavigator.error">\r\n        <a href="" class="thumbnail text-center" ng-click="smartClick(item)"  title="{{item.model.name}} ({{item.model.sizeKb()}}kb)">\r\n            <div class="item-icon">\r\n                <i class="glyphicon glyphicon-folder-open" ng-show="item.model.type === \'dir\'"></i>\r\n                <i class="glyphicon glyphicon-file" ng-show="item.model.type === \'file\'"></i>\r\n            </div>\r\n            {{item.model.name | strLimit : 11 }}\r\n        </a>\r\n    </div>\r\n\r\n    <div ng-show="fileNavigator.requesting">\r\n        <div ng-include="config.tplPath + \'/spinner.html\'"></div>\r\n    </div>\r\n\r\n    <div class="alert alert-warning" ng-show="!fileNavigator.requesting && fileNavigator.fileList.length < 1 && !fileNavigator.error">\r\n        {{"no_files_in_folder" | translate}}...\r\n    </div>\r\n    \r\n    <div class="alert alert-danger" ng-show="!fileNavigator.requesting && fileNavigator.error">\r\n        {{ fileNavigator.error }}\r\n    </div>\r\n</div>')
//    , e.put("src/templates/main-table-modal.html", '<table class="table table-condensed table-modal-condensed mb0">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                <a href="" ng-click="order(\'model.name\')">\r\n                    {{"name" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.name\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="text-right"></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody class="file-item">\r\n        <tr ng-show="fileNavigator.requesting">\r\n            <td colspan="2">\r\n                <div ng-include="config.tplPath + \'/spinner.html\'"></div>\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && !fileNavigator.listHasFolders() && !fileNavigator.error">\r\n            <td colspan="2">\r\n                {{"no_folders_in_folder" | translate}}...\r\n            </td>\r\n            <td class="text-right">\r\n                <button class="btn btn-sm btn-default" ng-click="fileNavigator.upDir()">{{"go_back" | translate}}</button>\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && fileNavigator.error">\r\n            <td colspan="2">\r\n                {{ fileNavigator.error }}\r\n            </td>\r\n        </tr>\r\n        <tr ng-repeat="item in fileNavigator.fileList | orderBy:predicate:reverse" ng-show="!fileNavigator.requesting && item.model.type === \'dir\'">\r\n            <td>\r\n                <a href="" ng-click="fileNavigator.folderClick(item)" title="{{item.model.name}} ({{item.model.sizeKb()}}kb)">\r\n                    <i class="glyphicon glyphicon-folder-close"></i>\r\n                    {{item.model.name | strLimit : 32}}\r\n                </a>\r\n            </td>\r\n            <td class="text-right">\r\n                <button class="btn btn-sm btn-default" ng-click="select(item, temp)">\r\n                    <i class="glyphicon glyphicon-hand-up"></i> {{"select_this" | translate}}\r\n                </button>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>'),
//    e.put("src/templates/main-table.html", '<table class="table mb0 table-files">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                <a href="" ng-click="order(\'model.name\')">\r\n                    {{"name" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.name\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="hidden-xs">\r\n                <a href="" ng-click="order(\'model.size\')">\r\n                    {{"size" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.size\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="hidden-sm hidden-xs">\r\n                <a href="" ng-click="order(\'model.date\')">\r\n                    {{"date" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.date\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="hidden-sm hidden-xs">\r\n                <a href="" ng-click="order(\'model.permissions\')">\r\n                    {{"permissions" | translate}}\r\n                    <span class="sortorder" ng-show="predicate[1] === \'model.permissions\'" ng-class="{reverse:reverse}"></span>\r\n                </a>\r\n            </th>\r\n            <th class="text-right"></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody class="file-item">\r\n        <tr ng-show="fileNavigator.requesting">\r\n            <td colspan="5">\r\n                <div ng-include="config.tplPath + \'/spinner.html\'"></div>\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && fileNavigator.fileList.length < 1 && !fileNavigator.error">\r\n            <td colspan="5">\r\n                {{"no_files_in_folder" | translate}}...\r\n            </td>\r\n        </tr>\r\n        <tr ng-show="!fileNavigator.requesting && fileNavigator.error">\r\n            <td colspan="5">\r\n                {{ fileNavigator.error }}\r\n            </td>\r\n        </tr>\r\n        <tr ng-repeat="item in fileNavigator.fileList | filter: {model:{name: query}} | orderBy:predicate:reverse" ng-show="!fileNavigator.requesting">\r\n            <td ng-right-click="touch(item)">\r\n                <a href="" ng-click="smartClick(item)" title="{{item.model.name}} ({{item.model.sizeKb()}}kb)">\r\n                    <i class="glyphicon glyphicon-folder-close" ng-show="item.model.type === \'dir\'"></i>\r\n                    <i class="glyphicon glyphicon-file" ng-show="item.model.type === \'file\'"></i>\r\n                    {{item.model.name | strLimit : 64}}\r\n                </a>\r\n            </td>\r\n            <td class="hidden-xs">\r\n                {{item.model.sizeKb()}}kb\r\n            </td>\r\n            <td class="hidden-sm hidden-xs">\r\n                {{item.model.date | formatDate }}\r\n            </td>\r\n            <td class="hidden-sm hidden-xs">\r\n                {{item.model.perms.toCode(item.model.type === \'dir\'?\'d\':\'-\')}}\r\n            </td>\r\n            <td class="text-right">\r\n                <div ng-include="config.tplPath + \'/item-toolbar.html\'"></div>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>'),
//    e.put("src/templates/main.html", '<div ng-controller="FileManagerCtrl">\r\n    <div ng-include="config.tplPath + \'/navbar.html\'"></div>\r\n\r\n    <div class="container-fluid">\r\n        <div class="row">\r\n\r\n            <div class="col-sm-3 col-md-2 sidebar file-tree animated slow fadeIn" ng-include="config.tplPath + \'/sidebar.html\'" ng-show="config.sidebar && fileNavigator.history[0]"></div>\r\n            <div class="main" ng-class="config.sidebar && fileNavigator.history[0] && \'col-sm-9 col-sm-offset-3 col-md-10 col-md-offset-2\'">\r\n                <div ng-include="config.tplPath + \'/current-folder-breadcrumb.html\'" ng-show="config.breadcrumb"></div>\r\n                <div ng-include="config.tplPath + \'/\' + viewTemplate" class="main-navigation clearfix"></div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n    <div ng-include="config.tplPath + \'/modals.html\'"></div>\r\n    <div ng-include="config.tplPath + \'/item-context-menu.html\'"></div>\r\n</div>'),
//    e.put("src/templates/modals.html", '<div class="modal animated fadeIn" id="imagepreview">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n    <form ng-submit="remove(temp)">\r\n      <div class="modal-header">\r\n        <button type="button" class="close" data-dismiss="modal">\r\n            <span aria-hidden="true">&times;</span>\r\n            <span class="sr-only">{{"close" | translate}}</span>\r\n        </button>\r\n        <h4 class="modal-title">{{"preview" | translate}}</h4>\r\n      </div>\r\n      <div class="modal-body" ng-show="temp.getUrl().length > 10">\r\n        <div class="text-center">\r\n          <img id="imagepreview-target" class="preview" alt="{{temp.model.name}}" ng-class="{\'loading\': temp.inprocess}">\r\n          <span class="label label-warning" ng-show="temp.inprocess">{{\'loading\' | translate}} ...</span>\r\n        </div>\r\n        <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n      </div>\r\n      <div class="modal-footer">\r\n        <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n      </div>\r\n      </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="delete">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n    <form ng-submit="remove(temp)">\r\n      <div class="modal-header">\r\n        <button type="button" class="close" data-dismiss="modal">\r\n            <span aria-hidden="true">&times;</span>\r\n            <span class="sr-only">{{"close" | translate}}</span>\r\n        </button>\r\n        <h4 class="modal-title">{{"confirm" | translate}}</h4>\r\n      </div>\r\n      <div class="modal-body">\r\n        {{\'sure_to_delete\' | translate}} <b>{{temp.model.name}}</b> ?\r\n        <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n      </div>\r\n      <div class="modal-footer">\r\n        <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n        <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess" autofocus="autofocus">{{"remove" | translate}}</button>\r\n      </div>\r\n      </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="rename">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="rename(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'change_name_move\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{\'enter_new_name_for\' | translate}} <b>{{temp.model.name}}</b></label>\r\n              <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n\r\n              <div ng-include data-src="\'path-selector\'" class="clearfix"></div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">{{\'rename\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="copy">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="copy(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'copy_file\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{\'enter_new_name_for\' | translate}} <b>{{temp.model.name}}</b></label>\r\n              <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n\r\n              <div ng-include data-src="\'path-selector\'" class="clearfix"></div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">Copy</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="compress">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="compress(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'compress\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <div class="label label-success error-msg">{{\'compression_started\' | translate}}</div>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <div ng-hide="config.allowedActions.compressChooseName">\r\n                    {{\'sure_to_start_compression_with\' | translate}} <b>{{temp.model.name}}</b> ?\r\n                  </div>\r\n                  <div ng-show="config.allowedActions.compressChooseName">\r\n                    <label class="radio">{{\'enter_folder_name_for_compression\' | translate}} <b>{{fileNavigator.currentPath.join(\'/\')}}/{{temp.model.name}}</b></label>\r\n                    <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n                  </div>\r\n              </div>\r\n\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n                  <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">{{\'compress\' | translate}}</button>\r\n              </div>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="extract" ng-init="temp.emptyName()">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="extract(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'extract_item\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <div class="label label-success error-msg">{{\'extraction_started\' | translate}}</div>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <label class="radio">{{\'enter_folder_name_for_extraction\' | translate}} <b>{{temp.model.name}}</b></label>\r\n                  <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n                  <div ng-include data-src="\'path-selector\'" class="clearfix"></div>\r\n              </div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <div ng-show="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n              </div>\r\n              <div ng-hide="temp.asyncSuccess">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n                  <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">{{\'extract\' | translate}}</button>\r\n              </div>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="edit" ng-class="{\'modal-fullscreen\': fullscreen}">\r\n  <div class="modal-dialog modal-lg">\r\n    <div class="modal-content">\r\n        <form ng-submit="edit(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <button type="button" class="close mr5" ng-click="fullscreen=!fullscreen">\r\n                  <span>&loz;</span>\r\n                  <span class="sr-only">{{\'toggle_fullscreen\' | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'edit_file\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n                <label class="radio">{{\'file_content\' | translate}}</label>\r\n                <span class="label label-warning" ng-show="temp.inprocess">{{\'loading\' | translate}} ...</span>\r\n                <textarea class="form-control code" ng-model="temp.tempModel.content" ng-show="!temp.inprocess" autofocus="autofocus"></textarea>\r\n                <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{\'close\' | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-show="config.allowedActions.edit" ng-disabled="temp.inprocess">{{\'edit\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="newfolder">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="createFolder(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'create_folder\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{\'folder_name\' | translate}}</label>\r\n              <input class="form-control" ng-model="temp.tempModel.name" autofocus="autofocus">\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-disabled="temp.inprocess">{{\'create\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="uploadfile">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="uploadFiles()">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{"upload_file" | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <label class="radio">{{"files_will_uploaded_to" | translate}} <b>{{fileNavigator.currentPath.join(\'/\')}}</b></label>\r\n              <input type="file" class="form-control" ng-file="$parent.uploadFileList" autofocus="autofocus" multiple="multiple"/>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <div ng-show="!fileUploader.requesting">\r\n                  <button type="button" class="btn btn-default" data-dismiss="modal">{{"cancel" | translate}}</button>\r\n                  <button type="submit" class="btn btn-primary" ng-disabled="!uploadFileList.length || fileUploader.requesting">{{\'upload\' | translate}}</button>\r\n              </div>\r\n              <div ng-show="fileUploader.requesting">\r\n                  <span class="label label-warning">{{"uploading" | translate}} ...</span>\r\n              </div>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="changepermissions">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n        <form ng-submit="changePermissions(temp)">\r\n            <div class="modal-header">\r\n              <button type="button" class="close" data-dismiss="modal">\r\n                  <span aria-hidden="true">&times;</span>\r\n                  <span class="sr-only">{{"close" | translate}}</span>\r\n              </button>\r\n              <h4 class="modal-title">{{\'change_permissions\' | translate}}</h4>\r\n            </div>\r\n            <div class="modal-body">\r\n              <table class="table mb0">\r\n                  <thead>\r\n                      <tr>\r\n                          <th>{{\'permissions\' | translate}}</th>\r\n                          <th class="col-xs-1 text-center">{{\'exec\' | translate}}</th>\r\n                          <th class="col-xs-1 text-center">{{\'read\' | translate}}</th>\r\n                          <th class="col-xs-1 text-center">{{\'write\' | translate}}</th>\r\n                      </tr>\r\n                  </thead>\r\n                  <tbody>\r\n                      <tr ng-repeat="(permTypeKey, permTypeValue) in temp.tempModel.perms">\r\n                          <td>{{permTypeKey | translate}}</td>\r\n                          <td ng-repeat="(permKey, permValue) in permTypeValue" class="col-xs-1 text-center" ng-click="main()">\r\n                              <label class="col-xs-12">\r\n                                <input type="checkbox" ng-model="temp.tempModel.perms[permTypeKey][permKey]">\r\n                              </label>\r\n                          </td>\r\n                      </tr>\r\n                </tbody>\r\n              </table>\r\n              <div class="checkbox" ng-show="config.enablePermissionsRecursive && temp.model.type === \'dir\'">\r\n                <label>\r\n                  <input type="checkbox" ng-model="temp.tempModel.recursive"> {{\'recursive\' | translate}}\r\n                </label>\r\n              </div>\r\n              <div class="clearfix mt10">\r\n                  <span class="label label-primary pull-left">\r\n                    {{\'original\' | translate}}: {{temp.model.perms.toCode(temp.model.type === \'dir\'?\'d\':\'-\')}} ({{temp.model.perms.toOctal()}})\r\n                  </span>\r\n                  <span class="label label-primary pull-right">\r\n                    {{\'changes\' | translate}}: {{temp.tempModel.perms.toCode(temp.model.type === \'dir\'?\'d\':\'-\')}} ({{temp.tempModel.perms.toOctal()}})\r\n                  </span>\r\n              </div>\r\n              <div ng-include data-src="\'error-bar\'" class="clearfix"></div>\r\n            </div>\r\n            <div class="modal-footer">\r\n              <button type="button" class="btn btn-default" data-dismiss="modal">{{"cancel" | translate}}</button>\r\n              <button type="submit" class="btn btn-primary" ng-disabled="">{{\'change\' | translate}}</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<div class="modal animated fadeIn" id="selector" ng-controller="ModalFileManagerCtrl">\r\n  <div class="modal-dialog">\r\n    <div class="modal-content">\r\n      <div class="modal-header">\r\n        <button type="button" class="close" data-dismiss="modal">\r\n            <span aria-hidden="true">&times;</span>\r\n            <span class="sr-only">{{"close" | translate}}</span>\r\n        </button>\r\n        <h4 class="modal-title">{{"select_destination_folder" | translate}}</h4>\r\n      </div>\r\n      <div class="modal-body">\r\n        <div>\r\n            <div ng-include="config.tplPath + \'/current-folder-breadcrumb.html\'"></div>\r\n            <div ng-include="config.tplPath + \'/main-table-modal.html\'"></div>\r\n        </div>\r\n      </div>\r\n      <div class="modal-footer">\r\n        <button type="button" class="btn btn-default" data-dismiss="modal" ng-disabled="temp.inprocess">{{"close" | translate}}</button>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<script type="text/ng-template" id="path-selector">\r\n  <div class="panel panel-primary mt10 mb0">\r\n    <div class="panel-body">\r\n        <div class="detail-sources">\r\n          <code class="mr5"><b>{{"source" | translate}}:</b> {{temp.model.fullPath()}}</code>\r\n        </div>\r\n        <div class="detail-sources">\r\n          <code class="mr5"><b>{{"destination" | translate}}:</b>{{temp.tempModel.fullPath()}}</code>\r\n          <a href="" class="label label-primary" ng-click="openNavigator(temp)">{{\'change\' | translate}}</a>\r\n        </div>\r\n    </div>\r\n  </div>\r\n</script>\r\n<script type="text/ng-template" id="error-bar">\r\n    <div class="label label-danger error-msg pull-left animated fadeIn" ng-show="temp.error">\r\n      <i class="glyphicon glyphicon-remove-circle"></i>\r\n      <span>{{temp.error}}</span>\r\n    </div>\r\n</script>'),
//    e.put("src/templates/navbar.html", '<nav class="navbar navbar-inverse navbar-fixed-top">\r\n  <div class="container-fluid">\r\n    <div class="navbar-header">\r\n      <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">\r\n        <span class="sr-only">Toggle</span>\r\n        <span class="icon-bar"></span>\r\n        <span class="icon-bar"></span>\r\n        <span class="icon-bar"></span>\r\n      </button>\r\n      <a class="navbar-brand hidden-xs" href="" ng-click="fileNavigator.goTo(-1)">{{config.appName}}</a>\r\n    </div>\r\n    <div id="navbar" class="navbar-collapse collapse">\r\n      <div class="navbar-form navbar-right">\r\n        <input type="text" class="form-control input-sm" ng-show="config.searchForm" placeholder="{{\'search\' | translate}}..." ng-model="$parent.query">\r\n        <button class="btn btn-default btn-sm" data-toggle="modal" data-target="#newfolder" ng-click="touch()">\r\n            <i class="glyphicon glyphicon-plus"></i> {{"create_folder" | translate}}\r\n        </button>\r\n        <button class="btn btn-default btn-sm" data-toggle="modal" data-target="#uploadfile" ng-show="config.allowedActions.upload" ng-click="touch()">\r\n            <i class="glyphicon glyphicon-upload"></i> {{"upload_file" | translate}}\r\n        </button>\r\n\r\n        <button class="btn btn-default btn-sm dropdown-toggle" type="button" id="dropDownMenuLang" data-toggle="dropdown" aria-expanded="true">\r\n            <i class="glyphicon glyphicon-globe"></i> {{"language" | translate}} <span class="caret"></span>\r\n        </button>\r\n        <ul class="dropdown-menu" role="menu" aria-labelledby="dropDownMenuLang">\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'en\')">{{"english" | translate}}</a></li>\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'es\')">{{"spanish" | translate}}</a></li>\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'pt\')">{{"portuguese" | translate}}</a></li>\r\n          <li role="presentation"><a role="menuitem" tabindex="-1" href="" ng-click="changeLanguage(\'fr\')">{{"french" | translate}}</a></li>\r\n        </ul>\r\n\r\n        <button class="btn btn-default btn-sm" ng-click="$parent.setTemplate(\'main-icons.html\')" ng-show="$parent.viewTemplate !== \'main-icons.html\'" title="{{\'icons\' | translate}}">\r\n            <i class="glyphicon glyphicon-th-large"></i>\r\n        </button>\r\n        <button class="btn btn-default btn-sm" ng-click="$parent.setTemplate(\'main-table.html\')" ng-show="$parent.viewTemplate !== \'main-table.html\'" title="{{\'list\' | translate}}">\r\n            <i class="glyphicon glyphicon-th-list"></i>\r\n        </button>\r\n\r\n      </div>\r\n    </div>\r\n  </div>\r\n</nav>'),
//        e.put("src/templates/sidebar.html", '<ul class="nav nav-sidebar file-tree-root">\r\n    <li ng-repeat="item in fileNavigator.history" ng-include="\'folder-branch-item\'" ng-class="{\'active\': item.name == fileNavigator.currentPath.join(\'/\')}"></li>\r\n</ul>\r\n\r\n<script type="text/ng-template" id="folder-branch-item">\r\n    <a href="" ng-click="fileNavigator.folderClick(item.item)" class="animated fast fadeInDown">\r\n        <i class="glyphicon glyphicon-folder-close mr2" ng-hide="isInThisPath(item.name)"></i>\r\n        <i class="glyphicon glyphicon-folder-open mr2"  ng-show="isInThisPath(item.name)"></i>\r\n        {{ (item.name.split(\'/\').pop() || \'/\') | strLimit : 24 }}\r\n    </a>\r\n    <ul class="nav nav-sidebar">\r\n        <li ng-repeat="item in item.nodes" ng-include="\'folder-branch-item\'" ng-class="{\'active\': item.name == fileNavigator.currentPath.join(\'/\')}"></li>\r\n    </ul>\r\n</script>'), e.put("src/templates/spinner.html", '<div class="spinner-wrapper col-xs-12">\r\n    <svg class="spinner-container" style="width:65px;height:65px" viewBox="0 0 44 44">\r\n        <circle class="path" cx="22" cy="22" r="20" fill="none" stroke-width="4"></circle>\r\n    </svg>\r\n</div>')
//}]);