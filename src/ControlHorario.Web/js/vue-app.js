Vue.component("login", {
    template: '#login',
    props: ['url'],
    data() {
        return {
            video: {},
            canvas: {},
            loggedIn: false,
            isCustomDate: false
        }
    },
    mounted: function () {
        this.video = this.$refs.video;
        navigator.mediaDevices.getUserMedia({video: true, audio: false})
            .then(function(stream) {
                this.video.srcObject = stream;
                this.video.play();
            })
            .catch(function(err) {
                console.log("An error occurred: " + err);
            });
    },
    methods: {
        capture() {
            const self = this;

            var loginRender = function(person){
                self.loggedIn = true;
                self.isCustomDate = false;
                self.$emit("login", person);
            }

            var logoutRender = function(){
                self.loggedIn = false;
                self.isCustomDate = false;
                self.$emit("logout");
            }

            if(this.loggedIn)
                logoutRender();
            else{
                this.canvas = this.$refs.canvas;
                const context = this.canvas.getContext("2d");
                context.drawImage(this.video, 0, 0, this.canvas.width, this.canvas.height);
                const capture = canvas.toDataURL("image/png");

                const config = { headers: {'Content-Type': 'application/json'} };
                axios.post(this.url, "\"" + capture + "\"", config)
                    .then(function (response) {
                        loginRender(response.data);
                    })
                    .catch(function (error) {
                        console.log(error);
                        logoutRender();
                    });
            }
        }
    }
});

Vue.component("person-data", {
    template: '#person-data',
    props:['person']
});

Vue.component("alert", {
    template: '#alert'
});

Vue.component("records-table", {
    template: '#records-table',
    props:['records'],
    computed: {
        Rows: function(){
            const result = this.records.map(function(r) {
                r['dateStr'] = new Date(r.dateTimeUtc).toLocaleString()
                return r;
            });
            return result;
        }
    }
});

Vue.component("records-table-item", {
    template: '#records-table-item',
    props:['record']
});

var app = new Vue({
    el: '#app',
    props: {
        url: {
            type: String,
            default: 'https://localhost:44377/'
        }
    },
    methods: {
        getAuthUrl(){
            return this.url + 'api/Person/identifybydata'
        },
        getPersonUrl(){
            return this.url + 'api/Person/'
        },
        onLogout(){
            this.person = null;
            this.records = null;
        },
        onLogin(token){
            this.person = token;
            this.getRecords(this.person.id)
        },
        onAddRecord(personId, date, isStart){
            const self = this;
            const api = self.getPersonUrl() + personId + '/record';
            axios.post(api, new {
                DateTimeUtc: date,
                IsStart: isStart
            })
                .then(function (response) {
                    self.records.push(response.data);
                })
                .catch(function (error) {
                    console.log(error);
                });
        },
        getRecords(personId, from, to){
            const self = this;
            let api = self.getPersonUrl() + personId + '/record';
            if(from && to)
                api += "?from=" + from + "&to=" + to;
            
            axios.get(api)
                .then(function (response) {
                    self.records = response.data;
                })
                .catch(function (error) {
                    console.log(error);
                    self.records = null;
                });
        }
    },
    data: {
        person: null,
        records: null
    }
});