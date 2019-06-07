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
        },
        addRecordClick(isStart){
            if(!this.loggedIn)
                return;
         
            const record = {
                date: new Date(),
                isStart  
            };
            this.$emit("addrecord", record);
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
    data(){
        return{
            isValid: true,
            periodType: 'today'
        }
    }, 
    computed: {
        Rows: function(){
            var self = this;
            let isStart = true;
            const result = this.records.map(function(item, index) {
                item['dateStr'] = new Date(item.dateTimeUtc).toLocaleString()
                item['isOk'] = item.isStart === isStart;

                if(item['isOk'])
                    isStart = !isStart;
                else
                    self.isValid = false;

                if(index + 1 == self.records.length
                    && item.isStart === true)
                {
                    self.isValid = false;
                    item['isOk'] = false;
                }

                return item;
            });
            return result;
        }
    },
    methods: {
        getRecords(period){
            if(!period)
                return;
            this.periodType = period;
            this.$emit("loadrecords", {
                periodType: period
            });
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

            this.onGetRecords({
                periodType: "today"
            })
        },
        onAddRecord(record){
            if(!record)
                return;

            const self = this;
            const personId =  record.personId || self.person.id;
            const api = self.getPersonUrl() + personId + '/record';
            
            axios.post(api, {
                DateTimeUtc: record.date.toISOString(),
                IsStart: record.isStart
            })
                .then(function (response) {
                    const records = self.records;
                    records.push(response.data);
                    self.records = records.sort(function(a,b){ 
                        if(a.dateTimeUtc > b.dateTimeUtc)
                            return 1;
                        else if(a.dateTimeUtc < b.dateTimeUtc)
                            return -1;
                        else if(a.isStart)
                            return 1;
                        return 0;
                    });
                })
                .catch(function (error) {
                    console.log(error);
                });
        },
        onGetRecords(filter){
            if(!filter)
                return;

            const date = new Date();
            let from = null;
            let to = null;

            switch(filter.periodType){
                case "today":
                    from = new Date(date.getFullYear(), date.getMonth(), date.getDate());
                    to = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 23, 59, 59);
                    break;
                case "yesterday":
                    date.setDate(date.getDate()-1);
                    from = new Date(date.getFullYear(), date.getMonth(), date.getDate());
                    to = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 23, 59, 59);
                    break;
            }

            const self = this;
            let api = self.getPersonUrl() + self.person.id + '/record';
            if(from && to)
                api += "?from=" + from.toISOString() + "&to=" + to.toISOString();
            
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