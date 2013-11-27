'use strict';

app.factory('Notes', ['$http', function ($http) {
    var url = 'api/note/';

    return {
        getSomeNotesAfterThisMany: function (count) {
            return $http.get(url + 'getsomenotesafterthismany?count= ' + count);
        },
        addNote: function (note) {
            return $http.post(url + 'note', note);
        }
    };
}]);