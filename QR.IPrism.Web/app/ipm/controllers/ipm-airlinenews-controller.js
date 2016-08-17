
'use strict';
google.load("feeds", "1");

angular.module('app.ipm.module')

	
	.directive('listDone', [ function () {
	    return function (scope, element, attrs) {
	        if (scope.$last) { // all are rendered
	            scope.$eval(attrs.listDone);
	        }
	    }
	}])

	.directive('onEnter',[function () {
	    return function (scope, element, attrs) {
	        element.on('keydown', function (event) {
	            if (event.which === 13) {
	                scope.$apply(attrs.onEnter);
	            }
	        })
	    }
	}])

	.service('rssFeed',[ function ($q, $rootScope) {
	    this.get = function (url) {
	        var d = $q.defer();
	        var feed = new google.feeds.Feed(url);
	        feed.setNumEntries(10);
	        feed.load(function (result) {
	            $rootScope.$apply(d.resolve(result));
	        });
	        return d.promise;
	    }
	}])

	.filter('strip_http', function () {
	    return function (str) {
	        var http = "http://";
	        return (str.indexOf(http) == 0) ? str.substr(http.length) : str;
	    }
	})

	.controller('AppCtrl', function ($scope, $location, $timeout, rssFeed) {
	    $scope.feedList = [
			'https://www.flightglobal.com/rss/news/airlines/'

	    ];

	    $scope.scrollPos = {}; // scroll position of each view

	    $(window).on('scroll', function () {
	        if ($scope.okSaveScroll) { // false between $routeChangeStart and $routeChangeSuccess
	            $scope.scrollPos[$location.path()] = $(window).scrollTop();
	          
	        }
	    });

	    $scope.scrollClear = function (path) {
	        $scope.scrollPos[path] = 0;
	    }

	    $scope.$on('$routeChangeStart', function () {
	        $scope.okSaveScroll = false;
	    });

	    $scope.$on('$routeChangeSuccess', function () {
	        $timeout(function () { // wait for DOM, then restore scroll position
	           
	            $scope.okSaveScroll = true;
	        }, 100);
	    });

	    $scope.setLoading = function (loading) {
	        $scope.isLoading = loading;
	    }

	    $scope.loadFeed = function (url, addFeed) {
	        $scope.setLoading(true);
	        rssFeed.get(url).then(function (result) {
	           
	            if (result.error) {
	                alert("ERROR " + result.error.code + ": " + result.error.message + "\nurl: " + url);
	                $scope.setLoading(false);
	            }
	            else {
	                if (addFeed) addFeed();
	                var urlParser = document.createElement('a');
	                urlParser.href = result.feed.link;
	                result.feed.viewAt = urlParser.hostname;
	                $scope.feed_result = result.feed;
	                $scope.scrollClear('/');
	                
	                if ($scope.feed_result.entries == 0) {
	                    $scope.setLoading(false);
	                }
	            }
	        });
	    }

	    $scope.mediaOne = function (entry) { // return first media object for 'entry'
	        return (entry && entry.mediaGroups) ? entry.mediaGroups[0].contents[0] : { url: '' };
	    }

	    $scope.hasVideo = function (entry) {
	        var media = $scope.mediaOne(entry);
	        return media.type ? (media.type == "video/mp4") : (media.url ? (media.url.indexOf(".mp4") != -1) : false);
	    }

	    $scope.ifPathNot = function (path) {
	        return $location.path() != path;
	    }

	    $scope.setCurrEntry = function (entry) {
	        $scope.currEntry = entry;
	    }

	    $scope.loadFeed($scope.feedList[0]);
	})

	.controller('ListCtrl', function ($scope, $location, $timeout) {
	    $scope.layoutDone = function () {
	        $scope.setLoading(false);
	        $timeout(function () { // wait for DOM
	            $('a[data-toggle="tooltip"]').tooltip();
	        }, 100);
	    }

	    $scope.viewDetail = function (entry) {
	        $scope.setCurrEntry(entry);
	        $location.path('/detail');
	    }
	})

	.controller('DetailCtrl', function ($scope, $location) {
	    $scope.scrollClear($location.path());

	    $scope.vPlayer = $('#vPlayer')[0];
	    $scope.videoPlay = $scope.hasVideo($scope.currEntry); // show errors only after "Play" video
	    $($scope.vPlayer).on('error', function () {
	        if ($scope.videoPlay) {
	            $scope.vidTagAlert.show();
	        }
	    });

	    $scope.vidTagAlert = $('#vidTagAlert');
	    $('#btnTagAlert').on('click', function () {
	        $scope.vidTagAlert.hide();
	    });

	    $scope.videoStop = function () {
	        $scope.vPlayer.pause();
	    }
	})

	.controller('FeedCtrl', function ($scope, $timeout) {
	    $scope.addFeed = function () {
	        var http = "http://";
	        var url = $scope.newFeedUrl;
	        if (url.indexOf(http) == -1) {
	            url = http + url; // add http if missing
	        }
	        $scope.loadFeed(url, function () {
	            $scope.feedList.unshift(url); // add to list of feeds
	        });
	    }

	    $scope.removeFeed = function (idx) {
	        $scope.feedList.splice(idx, 1);
	    }

	    $scope.chooseFeed = function (idx) {
	        $scope.feedList.splice(0, 0, $scope.feedList.splice(idx, 1)[0]); // move to top
	        $scope.loadFeed($scope.feedList[0]);
	    }

	    $scope.layoutDone = function () {
	        $timeout(function () { // wait for DOM
	            $('a[data-toggle="tooltip"]').tooltip();
	        }, 100);
	    }
	})
