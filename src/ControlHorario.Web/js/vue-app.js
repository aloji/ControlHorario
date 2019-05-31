Vue.component("login", {
    template: '#login',
    props: ['url'],
    data() {
        return {
            video: {},
            canvas: {}
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
            this.canvas = this.$refs.canvas;
            const context = this.canvas.getContext("2d")
                .drawImage(this.video, 0, 0, this.canvas.width, this.canvas.height);
            const capture = canvas.toDataURL("image/png");

            const config = { headers: {'Content-Type': 'application/json'} };
            axios.post(this.url, "\"" + capture + "\"", config)
                .then(function (response) {
                    console.info(response);
                })
                .catch(function (error) {
                    console.error(error);
                });
        }
    }
});

Vue.component("person-data", {
    template: '#person-data',
    props:['person']
});

Vue.component("records-table", {
    template: '#records-table',
    props:['records'],
    data: function() {
        return {
            rows: this.records
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
        }
    },
    data: {
        person: {
            Id: 'Guid',
            Name: 'Person Name'
        },
        records: [
            {
                Id: 'Guid-1',
                PersonId: 'Guid-2',
                DateTimeUtc: new Date(),
                IsStart: true
            },
            {
                Id: 'Guid-3',
                PersonId: 'Guid-4',
                DateTimeUtc: new Date(),
                Type: false
            },
        ]
    }
});