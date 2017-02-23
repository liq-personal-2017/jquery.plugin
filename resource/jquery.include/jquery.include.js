/// <reference path="../jquery/jquery.1.9.1.js" />
//$.ajax({
//    url: cssurl,
//    async: false,
//    success: function (data)
//    {
//        $("head").append("<link>");
//        var css = $("head").children(":last");
//        css.attr({
//            media: "all",
//            rel: "stylesheet",
//            type: "text/css",
//            href: cssurl
//        });
//    }
//});

//$.ajax({
//    type: 'GET',
//    url: '//' + rootip + '/' + rootpath + '/bootstrap-wizard/bootstrap-wizard' + minstr + '.js',
//    async: false,
//    dataType: 'script'
//});
; (function ()
{

    var browserVersion = (window.getBrowerVersion || function ()
    {

        try
        {
            var _browserMatch = '';

            var userAgent = navigator.userAgent,
            rMsie = /(msie\s|trident.*rv:)([\w.]+)/,
            rFirefox = /(firefox)\/([\w.]+)/,
            rOpera = /(opera).+version\/([\w.]+)/,
            rChrome = /(chrome)\/([\w.]+)/,
            rSafari = /version\/([\w.]+).*(safari)/;


            var browser;
            var version;
            var ua = userAgent.toLowerCase();

            var uaMatch = function (ua)
            {

                var match = rMsie.exec(ua);
                if (match != null)
                {
                    return { browser: "IE", version: match[2] || "0" };
                }
                var match = rFirefox.exec(ua);
                if (match != null)
                {
                    return { browser: match[1] || "", version: match[2] || "0" };
                }
                var match = rOpera.exec(ua);
                if (match != null)
                {
                    return { browser: match[1] || "", version: match[2] || "0" };
                }
                var match = rChrome.exec(ua);
                if (match != null)
                {
                    return { browser: match[1] || "", version: match[2] || "0" };
                }
                var match = rSafari.exec(ua);
                if (match != null)
                {
                    return { browser: match[2] || "", version: match[1] || "0" };
                }
                if (match != null)
                {
                    return { browser: "", version: "0" };
                }
            }


            _browserMatch = uaMatch(userAgent.toLowerCase());

            if (_browserMatch == undefined || _browserMatch == null)
            {
                _browserMatch = { browser: "", version: "" };
            }
            return _browserMatch;
            //if (browserMatch.browser)
            //{
            //    browser = browserMatch.browser;
            //    version = browserMatch.version;
            //}

            // return browser + '_' + version;

        }
        catch (ex)
        {
            _browserMatch = { browser: "", version: "" };
            return _browserMatch
        }

    })();
    //是否允许缓存
    var cache = true;
    //获取head元素
    var head = $('head');
    //计数器
    var count2Done = 0;
    function getScript(url)
    {
        //第一种方式，这种方式获取script不能跨域
        //$.ajax({
        //    type: 'GET',
        //    url: url,
        //    async: false,
        //    dataType: 'script',
        //    cache:true
        //});

        //这种方式最后jquery的解析结果跟第一种是相同的，差别在于这种方式不能缓存（jq内部写死了）
        //$("head").append('<script src="' + url + '" type="text/javascript"/>');

        //这个跟上一种没区别
        //$('<script>').attr('src', url).appendTo(head);

        //这个不管用
        //document.write('<script src="' + url + '" type="text/javascript"/>');//这个方式不能获取到对应的script
        
        count2Done++;
        var script = document.createElement('script');
        script.src = url;
        if (browserVersion.browser == 'IE' && +browserVersion.version.split('.')[0] <= 8)
        {
            //onload和onerror在ie8下不好使
            //ie8下失败了也是loaded
            script.onreadystatechange = function ()
            {
                if (this.readyState === 'loaded')
                {
                    count2Done--;
                    resolve();
                }
            }
        } else
        {
            script.onerror = script.onload = script.onreadystatechange = function ()
            { 
                count2Done--;
                resolve(); 
            }
        } 

        head[0].appendChild(script);
    }

    function resolve()
    {
        if (count2Done <= 0)
        {
            if ($.include.scriptReady)
            {
                $.include.scriptReady();
                $.include.scriptReady = null;
            }
        }

    }
    $.extend({
        include: function (option)
        {
              
            if (option == undefined || option == null)
            {
                option = {};
            }
            var libraryip = '[localhostIp]';
            //var rootip = option.rootip == undefined || option.rootip == null || option.rootip == '' ? '127.0.0.1' : option.rootip;
            //资源所在的项目名称，理论上来说是可以引用别的项目下的
            var rootpath = option.rootpath == undefined || option.rootpath == null || option.rootpath == '' ? 'jquery.plugin/resource' : option.rootpath;
            var ismin = option.ismin == undefined || option.ismin == null || option.ismin == '' ? false : option.ismin;
            var file = option.file == undefined || option.file == null || option.file == [] ? 'resource' : option.file;

            cache = option.cache && cache;

            var minstr = ismin == true ? '.min' : '';

            var files = typeof file == "string" ? [file] : file;
            var getCssKeys = "";
            var cssurl = '';
            for (var i = 0; i < files.length; i++)
            {
                var name = files[i];//files[i].replace(/^s|s$/g, "");
                switch (name)
                {
                    case "resource":
                        {
                            getCssKeys += 'resource^';
                        }
                        break;
  
                    case "bootstrap":   
                        {

                            getCssKeys += 'bootstrap^';

                        }
                        break;
                    case "common":
                        {

                            getCssKeys += 'common^';
                        }
                        break;
                    case "wheelmenu":
                        {

                            getCssKeys += 'wheelmenu^';
                        }

                        break;
                }
            }

            if (!(browserVersion.browser == 'IE' && +browserVersion.version.split('.')[0] <= 9) && getCssKeys != '')
            {
                getCssKeys = getCssKeys.substr(0, getCssKeys.length - 1);
                cssurl = '//' + libraryip + '/jquery.plugin/service/getCss.ashx?f=' + getCssKeys;
                $("head").append('<link rel="stylesheet" type="text/css" href="' + cssurl + '">');
            } else
            {
                getCssKeys = getCssKeys.substr(0, getCssKeys.length - 1);
                $.each(getCssKeys.split("^"), function (i, a)
                {
                    cssurl = '//' + libraryip + '/jquery.plugin/service/getCss.ashx?f=' + a;
                    $("head").append('<link rel="stylesheet" type="text/css" href="' + cssurl + '">');
                })
            }
            if (browserVersion.browser == 'IE' && +browserVersion.version.split('.')[0] <= 8)
            {
                getCssKeys = getCssKeys.substr(0, getCssKeys.length - 1);
                cssurl = '//' + libraryip + '/jquery.plugin/resource/theme/themeIE8' + minstr + '.css';
                $("head").append('<link rel="stylesheet" type="text/css" href="' + cssurl + '">');
            }
            getCssKeys = '';
            for (var i = 0; i < files.length; i++)
            {
                var name = files[i];//files[i].replace(/^s|s$/g, "");
                var jssrc = '';
                cssurl = '';
                switch (name)
                {
                   
                    case "resource":
                        { 
                            
                            //=================powerange================= //<!--checklist\radiolist\itemlist控件-->
                         
                            jssrc = '//' + libraryip + '/' + rootpath + '/jquery.list/jquery.itemlist' + minstr + '.js';
                            getScript(jssrc);
                             
                            jssrc = '//' + libraryip + '/' + rootpath + '/jquery.list/jquery.checklist' + minstr + '.js';
                            getScript(jssrc);
                             
                            jssrc = '//' + libraryip + '/' + rootpath + '/jquery.list/jquery.radiolist' + minstr + '.js';
                            getScript(jssrc);
                              
                            //=================jquery.fileuploader================= //

                            
                            jssrc = '//' + libraryip + '/' + rootpath + '/jquery.fileuploader/jquery.fileuploader' + minstr + '.js';
                            getScript(jssrc);
                           


                            getCssKeys += 'resource^';

                           

                        }
                        break;
                    case "bootstrap":
                        {
                            
                            jssrc = '//' + libraryip + '/' + rootpath + '/bootstrap/bootstrap' + minstr + '.js';
                            getScript(jssrc);
                             
                            getCssKeys += 'bootstrap^';

                        }
                        break;
                    case "common":
                        {
                        
                            jssrc = '//' + libraryip + '/' + rootpath + '/common/common' + minstr + '.js';
                            getScript(jssrc);
                                  
                            jssrc = '//' + libraryip + '/' + rootpath + '/cookie/cookie' + minstr + '.js';
                            getScript(jssrc);


                            getCssKeys += 'common^';
                        }
                        break;
                    
                    default:
                        {

                            var index = name.lastIndexOf('.');

                            var ext = name.substr(index + 1);
                            var rootname = name.substr(0, index);

                            var href = '';
                            if (name.indexOf('.min.') > -1)
                            {
                                if (ismin == true)
                                {
                                    href = name;
                                }
                                else
                                {
                                    href = name.replace('.min.', '.');
                                }

                            }
                            else
                            {
                                if (ismin == true)
                                {
                                    href = rootname + '.min.' + ext;
                                }
                                else
                                {
                                    href = name;
                                }
                            }


                            var isCSS = ext == "css";

                            if (isCSS)
                            {
                                $("head").append('<link rel="stylesheet" type="text/css" href="' + href + '">');

                            }
                            else
                            {
                                
                                getScript(href);

                            }
                        }
                        break;
                }
            }

            resolve();


        }
    });
})();