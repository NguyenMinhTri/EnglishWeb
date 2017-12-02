var YG = YG || function(e, t) {
    function o(e) {
        return "undefined" != typeof e
    }

    function n(e) {
        C = e
    }

    function i(e) {
        var t = "???";
        switch (Number(e)) {
            case L.NONE:
                t = "NONE";
                break;
            case L.YG:
                t = "YG";
                break;
            case L.PARTNER:
                t = "PARTNER"
        }
        return t
    }

    function a(e) {
        var t = "???";
        switch (Number(e)) {
            case Y.ADSENSE:
                t = "ADSENSE";
                break;
            case Y.OTHER:
                t = "OTHER"
        }
        return t
    }

    function r(e) {
        var t = "???";
        switch (Number(e)) {
            case H.TOP:
                t = "plc_top";
                break;
            case H.BOTTOM:
                t = "plc_bottom";
                break;
            case H.LEFT:
                t = "plc_left";
                break;
            case H.RIGHT:
                t = "plc_right"
        }
        return t
    }

    function l(e) {
        e > 1 && 50 > e && (M = e)
    }

    function s(e) {
        return O[e]
    }

    function c(e) {
        O[e].toggle()
    }

    function d(e) {
        O[e].close()
    }

    function u(e) {
        var t = document.createElement("div");
        return t.innerHTML = e, t.firstChild
    }

    function g() {
        q || (q = !0, e.addEventListener("message", p, !1))
    }

    function y(e) {
        try {
            return JSON.parse(e)
        } catch (t) {}
        return null
    }

    function p(e) {
        var o = e.origin || e.originalEvent.origin;
        if ("https://youglish.com" === o) {
            var n = y(e.data);
            if (n && n.wid && n.action) {
                if (!O[n.wid]) return;
                if (O[n.wid].serverHit(), n.action === W_ACTION.WIDGET_RESIZE) {
                    var a = n.update === t || 1 == n.update;
                    O[n.wid].resize(n.width, n.height, a)
                } else n.action === W_ACTION.ADV ? (S = 1 == n.is_partner, k = 1 == n.is_demo, console.log("------------"), console.log("Ads raw data: " + e.data), console.log("-> key used: " + n.key + " -isPartner: " + S + " -isDemo: " + k + " -adToDisplay: " + i(n.ads_display)), console.log("-> YGAds: " + n.ads_code + " -YGLocations: " + n.ads_loc), console.log("------------"), O[n.wid].setAdConfig(n.ads_display, y(n.ads_code), n.ads_loc, 1 == Number(n.show_logo))) : n.action === W_ACTION.SEARCH_DONE ? (O[n.wid].display(), O[n.wid].broadcast("onSearchDone", n)) : n.action === W_ACTION.VIDEO_CHANGE ? O[n.wid].broadcast("onVideoChange", n) : n.action === W_ACTION.CAPTION_CHANGE ? O[n.wid].broadcast("onCaptionChange", n) : n.action === W_ACTION.CAPTION_CONSUMED ? O[n.wid].broadcast("onCaptionConsumed", n) : n.action === W_ACTION.PLAYER_READY ? (O[n.wid].bReady = !0, O[n.wid].broadcast("onPlayerReady", n)) : n.action === W_ACTION.PLAYER_STATECHANGE ? O[n.wid].broadcast("onPlayerStateChange", n) : n.action === W_ACTION.ERROR ? +n.code == U.BOT ? O[n.wid].display(!1) : (+n.code == U.OUTDATED_BROWSER && (O[n.wid].display(), O[n.wid].resize(n.width, n.height, !1), N && clearTimeout(N), N = setTimeout(function() {
                    O[n.wid].close()
                }, 3e3)), O[n.wid].broadcast("onError", n)) : n.action === W_ACTION.UNREADY && (O[n.wid].bReady = !1)
            }
        }
    }

    function f(e, t, o, n, i, a, r, l, s, c, d, u, g, y, p, f, A, E, h, m, b, _, T, v) {
        var N = !1,
            R = a,
            O = i;
        a && Number(a) > 0 ? "inner" != r && (a = -1) : (N = !0, a = -1);
        var w = "https://youglish.com/search/" + (t ? escape(t) : "") + "/" + (o ? o.toLowerCase() : "all") + "/emb=1&e_id=" + e + ("&e_comp=" + n) + (null == h ? "" : "&e_start=" + h) + (a ? "&e_h=" + a : "") + (N ? "&e_notif_h=1" : "") + (v ? "&e_hidepwdby=1" : "") + (l ? "&e_bg_color=" + escape(l) : "") + (m ? "&e_partbg_color=" + escape(m) : "") + (b ? "&e_txt_color=" + escape(b) : "") + (_ ? "&e_kw_color=" + escape(_) : "") + (s ? "&e_link_color=" + escape(s) : "") + (c ? "&e_ttl_color=" + escape(c) : "") + (d ? "&e_cap_color=" + escape(d) : "") + (u ? "&e_marker_color=" + escape(u) : "") + (g ? "&e_query_color=" + escape(g) : "") + (y ? "&e_cap_size=" + y : "") + (p ? "&e_rest_mode=" + p : "") + (f ? "&e_vid_quality=" + f : "") + (A ? "&e_title=" + encodeURIComponent(A) : "") + (E ? "&e_toggle_ui=" + E : "") + (R ? "&e_cur_h=" + R : "") + (O ? "&e_cur_w=" + O : "") + (T ? "&e_client=" + T : "") + (C ? "&e_partner=" + C : "");
        return w
    }

    function A(e) {
        O[e.getId()] = e
    }

    function E() {
        O = {}
    }

    function h(e, t) {
        if (e) {
            var o = e.contentWindow || e.contentDocument;
            o.postMessage(JSON.stringify(t), "*")
        }
    }

    function m(e) {
        return "object" == typeof HTMLElement ? e instanceof HTMLElement : e && "object" == typeof e && null !== e && 1 === e.nodeType && "string" == typeof e.nodeName
    }

    function b() {
        E();
        for (var e = "a.youglish-widget", t = document.querySelectorAll(e), o = 0; o < t.length; o++) _(t[o])
    }

    function _(e) {
        var t = e.id,
            o = {
                width: e.getAttribute("width") || e.getAttribute("data-width"),
                height: e.getAttribute("height") || e.getAttribute("data-height"),
                components: e.getAttribute("data-components"),
                scroll: e.getAttribute("data-scroll"),
                backgroundColor: e.getAttribute("data-bkg-color"),
                linkColor: e.getAttribute("data-link-color"),
                titleColor: e.getAttribute("data-ttl-color"),
                captionColor: e.getAttribute("data-cap-color"),
                markerColor: e.getAttribute("data-marker-color"),
                queryColor: e.getAttribute("data-query-color"),
                captionSize: e.getAttribute("data-cap-size"),
                restrictionMode: e.getAttribute("data-rest-mode"),
                videoQuality: e.getAttribute("data-video-quality"),
                title: e.getAttribute("data-title"),
                toggleUI: e.getAttribute("data-toggle-ui"),
                autoStart: e.getAttribute("data-auto-start"),
                panelsBackgroundColor: e.getAttribute("data-panels-bkg-color"),
                textColor: e.getAttribute("data-text-color"),
                keywordColor: e.getAttribute("data-keyword-color"),
                client: e.getAttribute("data-client")
            },
            n = new v(t, o); + e.getAttribute("data-delay-load") || n.search(e.getAttribute("data-query"), e.getAttribute("data-accent"))
    }

    function T(e, t) {
        var o = "position: static;visibility: visible;display: inline-block;padding: 0px;border: none;max-width: 100%;margin-top: 0px;margin-bottom: 1px;";
        return e = e && Number(e) > 0 ? e + "px" : "100%", t = t && Number(t) > 0 ? t + "px" : "1px", o + "width: " + e + ";height: " + t
    }

    function v(t, n) {
        function i(e, t) {
            return (e & w) > 0 || t
        }

        function l(e, t, o, n, i) {
            return "<" + e + (t ? " id='" + t + "'" : "") + (o ? " class='" + o + "'" : "") + (n ? " style='" + n + "'" : "") + ">" + (i ? i : "") + "</" + e + ">"
        }

        function s(t, o) {
            if (this.bReady) F(), h(Q(), {
                source: "youglish",
                action: P_ACTION.SEARCH,
                query: t,
                accent: o
            });
            else {
                var n = Number(ge.components);
                if (!i(n, t)) return;
                var a = T(ge.width, ge.height),
                    r = f(ie, t, o ? o : ge.accent, ge.components, ge.width, ge.height, ge.scroll, ge.backgroundColor, ge.linkColor, ge.titleColor, ge.captionColor, ge.markerColor, ge.queryColor, ge.captionSize, ge.restrictionMode, ge.videoQuality, ge.title, ge.toggleUI, ge.autoStart, ge.panelsBackgroundColor, ge.textColor, ge.keywordColor, ge.client, ue),
                    s = ge.height && Number(ge.height) > 0 && "inner" != ge.scroll ? "" : "scrolling='no'",
                    c = (n & (I | P | x)) > 0,
                    d = "<div style='display:none;" + (c ? "border: 1px solid #bec3e4" : "") + "' class='ygPanel'>" + (c ? y(n) : "") + "<div class='ygContentEx' style='display:flex;'><div style='flex-grow: 0' class='plc_left'></div><div style='flex-grow: 1' class='ygContent'><div style='display:flex;flex-direction:column;align-items:center;margin-top: -90px;'><div style='height: 75px; overflow: hidden; width: 100%; background: white; position: absolute;' class='plc_top'></div><iframe id='" + re + "' class='ygFrame' style='margin-top: 0px; margin-bottom: 0px;  height: 360px !important; width: 100%; border: none;'" + s + " style='" + a + "'  src='" + r + "'></iframe><div style='width: 100%;height: 30px;background: white;position: absolute;bottom: 120px;' class='plc_bottom'></div></div></div><div style='flex-grow: 0;' class='plc_right'></div></div></div>",
                    u = l("div", null, "ygProgress", "padding:12px;border: 1px solid lightgrey;font-size:15px;font-weight: 100;font-style: italic", "Đang tải đoạn hội thoại...");
                d = l("div", ie, (n & I) > 0 ? "ygContainer ygDraggable" : "ygContainer", "background-color:white;z-index:1", u + "\n" + d), document.getElementById(ie).outerHTML = d, (n & I) > 0 && ("undefined" == typeof DragModule ? e.onDragModuleReady = function(e) {
                    e.registerAll()
                } : DragModule.registerAll()), ae = null
            }
            te()
        }

        function c(e, t) {
            return t ? e + "='" + t + "'" : ""
        }

        function d() {
            var t, o = {};
            de.display == L.YG ? (console.log("\nDisplaying YG ads..."), de.ygAds && (o = de.ygAds), de.ygLocations && (t = de.ygLocations), t || (t = S ? ce : G)) : (console.log("\nDisplaying PARTNER ads..."), o = se, t = ce);
            for (var n = 1; 8 >= n; n *= 2)
                if ((t & n) > 0) {
                    var i = o[n];
                    if (i || (i = o[0]), i || (i = B), k && i.type != Y.OTHER) {
                        console.log("...cant display paid ad on a demo account!");
                        continue
                    }
                    var l = r(n),
                        s = document.getElementById(ie).getElementsByClassName(l)[0];
                    i.responsive && (s.style.width = g(n, t) + "px"), console.log("...displaying ad unit at: " + l + " -responsive: " + i.responsive + " -type: " + a(i.type)), i.type == Y.ADSENSE ? (s.innerHTML = i.code ? i.code : "<ins class='adsbygoogle'" + c(" style", i.style) + c(" data-ad-client", i.client) + c(" data-ad-slot", i.slot) + c(" data-ad-format", i.format) + "></ins>", (adsbygoogle = e.adsbygoogle || []).push({})) : s.innerHTML = i.code
                }
            console.log("done.\n")
        }

        function g(e, t) {
            var o = 0;
            if (e == H.LEFT || e == H.RIGHT) {
                var n = H.LEFT | H.RIGHT,
                    i = M + ((t & n) == n ? -10 : 0);
                i > 50 && (i = 50);
                var a = document.getElementById(ie).offsetWidth;
                if (o = Math.floor(a * (i / 100)), 60 > o) return 60
            } else if (o = Q().offsetWidth, 90 > o) return 90;
            return o
        }

        function y(e) {
            var t = "<div style='font-size: 20px;padding: 4px;background-color: #3e4571;color: white;" + ((e & I) > 0 ? "cursor: move" : "") + "'>&nbsp;\n";
            return (e & x) > 0 && (t += "<span title='close widget' style='cursor:hand;cursor:pointer;float: right;color: #b9b9b9;margin-left:5px;line-height: 23px;font-size:30px' onclick='YG.close(\"" + ie + "\")'>&times</span>"), (e & P) > 0 && (t += "<span title='Show/hide widget' style='cursor:hand;cursor:pointer;float: right;color: #b9b9b9;margin-left:5px;line-height: 23px;' onclick='YG.toggle(\"" + ie + "\")'>&#9776</span>   "), t += "</div>"
        }

        function p() {
            F(), h(Q(), {
                source: "youglish",
                action: P_ACTION.PLAY
            })
        }

        function E() {
            F(), h(Q(), {
                source: "youglish",
                action: P_ACTION.REPLAY
            })
        }

        function b() {
            h(Q(), {
                source: "youglish",
                action: P_ACTION.PAUSE
            })
        }

        function _() {
            F(), h(Q(), {
                source: "youglish",
                action: P_ACTION.PREV
            })
        }

        function v() {
            F(), h(Q(), {
                source: "youglish",
                action: P_ACTION.NEXT
            })
        }

        function N(e) {
            F(), h(Q(), {
                source: "youglish",
                action: P_ACTION.DELTA,
                delta: e
            })
        }

        function C() {
            return ie
        }

        function R(e, t) {
            if (ye[e])
                for (var o = 0; o < ye[e].length; o++) ye[e][o](t)
        }

        function O(e, t) {
            "undefined" == typeof ye[e] && (ye[e] = []), ye[e].push(t)
        }

        function W(e, t) {
            if (ye[e])
                for (var o = 0; o < ye[e].length; o++)
                    if (ye[e][o] === t) {
                        ye[e].splice(o, 1);
                        break
                    }
        }

        function q() {
            var e = document.getElementById(ie).getElementsByClassName("ygContentEx")[0];
            null == e.style.display || "none" == e.style.display ? (e.style.display = "flex", p()) : (e.style.display = "none", b())
        }

        function z() {
            Q().style.display = "none"
        }

        function F() {
            Q().style.display = "block"
        }

        function V() {
            this.bReady = !1, document.getElementById(ie).innerHTML = ""
        }

        function Z(e) {
            e && (ce |= e.location, se[e.location] = e, (e.location == H.BOTTOM || e.location == H.RIGHT) && (ue = !0), console.log("Partner AD attached at location: " + r(e.location)))
        }

        function j(e) {
            ce = e, ue = (e & H.BOTTOM) > 0 || (e & H.RIGHT) > 0, this.bReady = !1
        }

        function J() {
            se = {}, ce = 0, ue = !1
        }

        function Q() {
            return document.getElementById(re)
        }

        function X(e, t, o) {
            var n = {
                    source: "youglish",
                    action: P_ACTION.SIZE
                },
                i = Q();
            t && Number(t) > 0 && (i.style.height = t + "px", n.height = t), e && Number(e) > 0 && (i.style.width = e + "px", n.width = e), o && h(i, n)
        }

        function K() {
            return de && (de.display == L.PARTNER || de.display == L.YG)
        }

        function $(e, t, o, n) {
            de = {
                display: Number(e),
                ygAds: t,
                ygLocations: Number(o)
            }, n && document.getElementById(ie).appendChild(u("<div style='margin-top: 15px;float: right;color: grey;font-size: 13px;font-style: oblique;padding: 2px 5px;border-top: 1px solid grey;'> Powered by <a style='text-decoration: none;color:#337ab7' href='https://youglish.com'>Youglish.com</a></div>"))
        }

        function ee(e) {
            var t = document.getElementById(ie),
                n = t.getElementsByClassName("ygPanel")[0];
            Ee && "block" == n.style.display || (t.getElementsByClassName("ygProgress")[0].style.display = "none", n.style.display = "block", K() && d(), Ee = o(e) ? e : !0)
        }

        function te() {
            Ae && clearTimeout(Ae), Ae = setTimeout(ne, 5e3)
        }

        function oe() {
            Ae && (clearTimeout(Ae), Ae = null)
        }

        function ne() {
            R("onError", {
                wid: ie,
                action: W_ACTION.ERROR,
                code: U.TIMEOUT
            })
        }
        var ie, ae, re, le = {},
            se = {},
            ce = 0,
            de = null,
            ue = !1;
        this.bReady = !1, ie = m(t) ? t.getAttribute("id") : t, re = "fr_" + ie;
        var ge = n || {},
            ye = {};
        if (ge.scroll = ge.scroll ? ge.scroll : "inner", ge.components = ge.components ? ge.components : 255, ge.events)
            for (var pe in ge.events) O(pe, ge.events[pe]);
        ge.query && s(ge.query, ge.accent);
        var fe = Number(ge.components);
        (fe & I) > 0 && !D && (D = !0, function() {
            var e = document.createElement("script");
            e.async = !0, e.src = "https://youglish.com/public/emb/ext_min.js";
            var t = document.getElementsByTagName("script")[0];
            t.parentNode.insertBefore(e, t)
        }());
        var Ae, Ee = !1;
        return le.broadcast = R, le.addEventListener = O, le.removeEventListener = W, le.search = s, le.replay = E, le.play = p, le.pause = b, le.previous = _, le.next = v, le.move = N, le.getId = C, le.resize = X, le.toggle = q, le.close = V, le.show = F, le.hide = z, le.display = ee, le.serverHit = oe, le.attachAd = Z, le.resetAds = J, le.setAdsLocation = j, le.setAdConfig = $, A(le), le
    }
    var N, C, R = {},
        O = {},
        w = 1,
        I = 1024,
        P = 2048,
        x = 4096,
        D = !1,
        S = !1,
        k = !1,
        L = {
            NONE: 0,
            YG: 1,
            PARTNER: 2
        },
        H = {
            LEFT: 1,
            RIGHT: 2,
            TOP: 4,
            BOTTOM: 8
        },
        Y = {
            ADSENSE: 0,
            OTHER: 1
        },
        G = H.LEFT,
        B = {
            type: Y.ADSENSE,
            responsive: !0,
            code: "<ins class='adsbygoogle' style='display:block' data-ad-client='ca-pub-4884889260645232' data-ad-slot='7940721009' data-ad-format='auto'></ins>"
        },
        M = 35,
        W = {
            UNSTARTED: -1,
            ENDED: 0,
            PLAYING: 1,
            PAUSED: 2,
            BUFFERING: 3,
            CUED: 5
        },
        U = {
            PLAYER: 1,
            OUTDATED_BROWSER: 2,
            TIMEOUT: 3,
            BOT: 4
        };
    W_ACTION = {
        ERROR: 1,
        WIDGET_RESIZE: 2,
        SERVER_HIT: 3,
        ADV: 4,
        SEARCH_DONE: 20,
        VIDEO_CHANGE: 21,
        CAPTION_CHANGE: 22,
        CAPTION_CONSUMED: 23,
        PLAYER_READY: 40,
        PLAYER_STATECHANGE: 41,
        UNREADY: 100
    }, P_ACTION = {
        PLAY: 1,
        REPLAY: 2,
        PAUSE: 3,
        PREV: 4,
        NEXT: 5,
        DELTA: 6,
        SEARCH: 7,
        SIZE: 8
    };
    var q = !1;
    return function() {
        g(), b()
    }(), R.PlayerState = W, R.Error = U, R.AdLocations = H, R.AdNetworks = Y, R.setParnterKey = n, R.parsePage = b, R.Widget = v, R.getWidget = s, R.toggle = c, R.close = d, R.setAdWidthRatio = l, R
}(window);
"function" == typeof onYouglishAPIReady && onYouglishAPIReady();