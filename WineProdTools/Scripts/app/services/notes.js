'use strict';

app.factory('Notes', ['$http', function ($http) {
    var url = 'api/note/';

    return {
        getNotes: function () {
            return $http.get(url + 'getnotes');
        },
        addNote: function (note) {
            return $http.post(url + 'note', note);
        }
    };
}]);