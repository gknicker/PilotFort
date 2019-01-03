$(document).ready(function () {
	$(".focus").focus();
});

var PilotFort = {};

// map display
PilotFort.mapHolderSelector = '#mapholder';
PilotFort.displayMap = function (latitude, longitude) {
	var latLng = latitude + "," + longitude;
	var apiKey = "YOUR_:KEY";
	var imgUrl = "https://maps.googleapis.com/maps/api/staticmap?center=" + latLng + "&zoom=14&size=400x300&sensor=false&key=" + apiKey;
	$(PilotFort.mapHolderSelector).html("<img src='" + imgUrl + "'>");
};

// time zone
PilotFort.getTimeZone = function () {
	if (Intl && typeof Intl.DateTimeFormat == 'function') {
		// gets an IANA time zone identifier, such as America/Los_Angeles
		return Intl.DateTimeFormat().resolvedOptions().timeZone;
	}
	// gets OS time zone identifier, might be Windows or IANA or other
	return /\((.*)\)/.exec(new Date().toString())[1];
};

/* geolocation
coords.latitude = The latitude as a decimal number (always returned)
coords.longitude = The longitude as a decimal number (always returned)
coords.accuracy = The accuracy of position (always returned)
coords.altitude = The altitude in meters above the mean sea level (returned if available)
coords.altitudeAccuracy = The altitude accuracy of position (returned if available)
coords.heading = The heading as degrees clockwise from North (returned if available)
coords.speed = The speed in meters per second (returned if available)
*/
PilotFort.geolocationSelector = '#geolocation';
PilotFort.geolocationOptions = {
	maximumAge: 0,
	enableHighAccuracy: true,
};
PilotFort.geolocation = function (selector) {
	if (selector) {
		PilotFort.geolocationSelector = selector;
	}
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(
			PilotFort.geolocationSuccess,
			PilotFort.geolocationFailure,
			PilotFort.geolocationOptions);
	} else {
		$(PilotFort.geolocationSelector).html("Geolocation is not supported by this browser.");
	}
};
PilotFort.geolocationFailure = function (error) {
	$(PilotFort.geolocationSelector).html(getMessage(error)).css('display', 'none');
	function getMessage(error) {
		switch (error.code) {
			case error.PERMISSION_DENIED:
				return "Geolocation request was denied by the user."
			case error.POSITION_UNAVAILABLE:
				return "Geolocation information is unavailable."
			case error.TIMEOUT:
				return "Geolocation request timed out."
			case error.UNKNOWN_ERROR:
				return "Geolocation unknown error occurred."
			default:
				return "Geolocation was not completed by the browser.";
		}
	}
};
PilotFort.geolocationSuccess = function (position) {
	var lat = position.coords.latitude;
	var lng = position.coords.longitude;
	var latDirection = lat == 0 ? '' : lat > 0 ? ' N' : ' S';
	var lngDirection = lng == 0 ? '' : lat > 0 ? ' E' : ' W';
	var latDegree = Math.floor(Math.abs(lat));
	var lngDegree = Math.floor(Math.abs(lng));
	var latMinutes = (Math.abs(lat) - latDegree) * 60;
	var lngMinutes = (Math.abs(lng) - lngDegree) * 60;
	var latMinute = Math.floor(latMinutes);
	var lngMinute = Math.floor(lngMinutes);
	var latSecond = Math.round((latMinutes - latMinute) * 60);
	var lngSecond = Math.round((lngMinutes - lngMinute) * 60);
	function leftPad(num) { return (num < 10 ? '0' : '') + num; }
	var html = '';
	html += latDegree + '&deg;';
	html += leftPad(latMinute) + '&prime;';
	html += leftPad(latSecond) + '&Prime;'
	html += latDirection + '&nbsp; ';
	html += lngDegree + '&deg;';
	html += leftPad(lngMinute) + '&prime;'
	html += leftPad(lngSecond) + '&Prime;'
	html += lngDirection + '&nbsp;';
	$(PilotFort.geolocationSelector).html(html);
};
