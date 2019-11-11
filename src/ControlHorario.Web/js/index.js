Vue.component("login", {
    template: '#login',
    props: ['url'],
    data() {
        return {
            video: {},
            canvas: {},
            loggedIn: false,
            isCustomDate: false,
            recordTime: new Date()
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
            self.$emit("removealert");

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

                self.$emit("showinfo", "loading....");

                const config = { headers: {'Content-Type': 'application/json'} };
                axios.post(this.url, "\"" + capture + "\"", config)
                    .then(function (response) {
                        loginRender(response.data);
                    })
                    .catch(function (error) {
                        console.log(error);
                        if(error.response.status === 404)
                            self.$emit("showerror", "user not identified");
                        else
                            self.$emit("showerror", error);
                        logoutRender();
                    });
            }
        },
        addRecordClick(isStart){
            if(!this.loggedIn)
                return;        
                
            const now = new Date();
            now.setSeconds(0);
            now.setMilliseconds(0);

            let date = this.isCustomDate ? new Date(this.recordTime) : now;
            const record = {
                date,
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

Vue.component("report-data", {
    template: '#report-data',
    props:['report']
});

Vue.component("alert", {
    template: '#alert',
    props: {
        error: null
    },
    computed: {
        classObject: function () {
            return  "alert-" + this.error.level;
        }
    }
});

Vue.component("records-table", {
    template: '#records-table',
    props:['records', 'period'],
    data(){
        return{
            isValid: true
        }
    }, 
    computed: {
        Rows: function(){
            var self = this;
            let isStart = true;
            const result = this.records.map(function(item, index) {

                const recordDate = new Date(item.dateTimeUtc);
                item['dateStr'] = recordDate.toLocaleString();
                item["canDelete"] = true;
                    
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
        },
        Hours: function(){
            let milliseconds = 0;
            if(this.records && this.records.length > 0 && !(this.records.length % 2))
                for (let index = this.records.length - 1; index > 0; index = index-2) {
                    const stop = this.records[index];
                    const start = this.records[index-1];
                    
                    const diffMillSec = new Date(stop.dateTimeUtc) - new Date(start.dateTimeUtc);

                    if(stop.isStart || !start.isStart || diffMillSec < 0){
                        milliseconds = 0;
                        break;
                    }
                    milliseconds += diffMillSec;
                }
            
            let result = 'N/A';
            if(milliseconds > 0)
            {
                const hours = Math.floor((milliseconds / (1000*60*60)));
                const minutes = Math.floor((milliseconds / (1000*60)) % 60);
                result = hours + 'h' + minutes + 'm'
            }
            return result;
        }
    },
    methods: {
        getRecords(periodType){
            if(!periodType)
                return;
            this.$emit("loadrecords", periodType);    
        },
        onDelete(record){
            this.$emit("removerecord", record);    
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
            this.records = [];
            this.periodType = null;
        },
        onLogin(token){
            this.person = token;
            this.onGetRecords('today');
            const period = new Date().getDay() === 1 ? 'previousweek' : 'yesterday';
            this.onGetReport(period);
        },
        getMonday(date) {
            const day = date.getDay();
            const diff = date.getDate() - day + (day == 0 ? -6:1);
            return new Date(date.setDate(diff));
        },
        getBeforeOneWeek(date){
            const weekMills = 60*60*24*7*1000;
            return new Date(date.getTime() - weekMills);
        },
        getFrom(period){
            const date = new Date();
            switch(period){
                case "today":
                    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
                case "yesterday":
                    date.setDate(date.getDate()-1);
                    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
                case "thisweek":
                    const monday = this.getMonday(date);
                    return new Date(monday.getFullYear(), monday.getMonth(), monday.getDate());
                case "thismonth":
                    return new Date(date.getFullYear(), date.getMonth(), 1);
                case "previousmonth":
                    return new Date(date.getFullYear(), date.getMonth() - 1, 1);
                case "previousweek":
                    const beforeOneWeek = this.getBeforeOneWeek(date);
                    const beforeMonday = this.getMonday(beforeOneWeek);
                    return new Date(beforeMonday.getFullYear(), beforeMonday.getMonth(), beforeMonday.getDate());
            }
        },
        getTo(period){
            const date = new Date();
            switch(period){
                case "today":
                    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), 23, 59, 59);
                case "yesterday":
                    date.setDate(date.getDate()-1);
                    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), 23, 59, 59);
                case "thisweek":
                    const monday = this.getMonday(date);
                    var endWeek = new Date(monday.setDate(monday.getDate() + 6));
                    return new Date(endWeek.getFullYear(), endWeek.getMonth(), endWeek.getDate(), 23, 59, 59);
                case "thismonth":
                    const lastDayThisMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
                    return new Date(date.getFullYear(), date.getMonth(), lastDayThisMonth, 23, 59, 59);
                case "previousmonth":
                    const lastDayPreMonth = new Date(date.getFullYear(), date.getMonth(), 0).getDate();
                    return new Date(date.getFullYear(), date.getMonth() - 1, lastDayPreMonth, 23, 59, 59);
                case "previousweek":
                    const beforeOneWeek = this.getBeforeOneWeek(date);
                    const beforeMonday = this.getMonday(beforeOneWeek);
                    var beforeEndWeek = new Date(beforeMonday.setDate(beforeMonday.getDate() + 6));
                    return new Date(beforeEndWeek.getFullYear(), beforeEndWeek.getMonth(), beforeEndWeek.getDate(), 23, 59, 59);
            }
        },
        onRemoveRecord(record){
            if(!record)
                return;
            
            const self = this;
            const personId =  record.personId || self.person.id;
            const api = self.getPersonUrl() + personId + '/record/' + record.id;

            self.onRemoveAlert();

            axios.delete(api)
                .then(function(){
                    const date = new Date(record.dateTimeUtc);
                    if(self.getFrom(self.periodType) <= date
                        && date <= self.getTo(self.periodType))
                    {
                        const r = self.records;
                        self.records = r.filter(el => el !== record);
                    }
                    self.onShowInfo("record deleted");
                })
                .catch(function (error) {
                    console.log(error);
                    self.onShowError(error);
                });

        },
        onAddRecord(record){
            if(!record)
                return;

            const self = this;
            const personId =  record.personId || self.person.id;
            const api = self.getPersonUrl() + personId + '/record';
            
            self.onRemoveAlert();

            axios.post(api, {
                DateTimeUtc: record.date.toISOString(),
                IsStart: record.isStart
            })
                .then(function (response) {
                    const date = new Date(response.data.dateTimeUtc);
                    if(self.getFrom(self.periodType) <= date
                        && date <= self.getTo(self.periodType)){
                        
                        const r = self.records;
                        r.push(response.data);
                        self.records = r.sort(function(a,b){ 
                            if(a.dateTimeUtc > b.dateTimeUtc)
                                return 1;
                            else if(a.dateTimeUtc < b.dateTimeUtc)
                                return -1;
                            else if(a.isStart)
                                return 1;
                            return 0;
                        });
                    }
                    self.onShowInfo("record added");
                })
                .catch(function (error) {
                    console.log(error);
                    self.onShowError(error);
                });
        },
        onGetRecords(period){
            if(!period || this.periodType === period)
                return;

            this.periodType = period;

            const from = this.getFrom(this.periodType);
            const to = this.getTo(this.periodType);
            const self = this;

            self.onRemoveAlert();

            let api = self.getPersonUrl() + self.person.id + '/record';
            if(from && to)
                api += "?from=" + from.toISOString() + "&to=" + to.toISOString();
            
            axios.get(api)
                .then(function (response) {
                    self.records = response.data;
                })
                .catch(function (error) {
                    console.log(error);
                    self.records = [];
                });
        },
        onGetReport(period){
            if(!period)
                return;
            
            const from = this.getFrom(period);
            const to = this.getTo(period);
            const self = this;

            let api = self.getPersonUrl() + self.person.id + '/report';
            if(from && to){
                api += "?from=" + from.toISOString() + "&to=" + to.toISOString();
                axios.get(api)
                    .then(function (response) {
                        let report = response.data;
                        report['period'] = period;
                        self.report = report;
                    })
                    .catch(function (error) {
                        console.log(error);
                        self.report = null;
                    });
            }
        },
        onShowError(msg){
            this.log = {
                level: "danger",
                msg: msg
            };
        },
        onShowInfo(msg){
            this.log = {
                level: "info",
                msg: msg
            };
        },
        onRemoveAlert(){
            this.log = null;
        },
    },
    data: {
        person: null,
        records: [],
        periodType: null,
        log: null,
        report: null
    }
});