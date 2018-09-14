import { Component, Pipe, PipeTransform, OnInit, HostListener, Input, ElementRef, Output, EventEmitter} from '@angular/core';
import { Observable } from 'rxjs/Rx';
declare var $: any;

@Component({
    selector: 'side-panel',
    templateUrl: "./sidepanel.component.html"
})

export class SidePanelComponent {
    @Input() nameGridRelatedToSidePanel: string;

    public sidePanelRowSelected: boolean = false;

    ngOnInit() {
        $(window).bind('load resize scroll', (e) => {
            this.resizeSidePanelContent();
        });
    }

    bindSidePanelControls() {
        $("div[ro-side-panel-toggle]").click((e) => {
            if ($(e.currentTarget).has(".xico-action-close").length) {
                this.closeSidePanel(e.currentTarget);
            }
            else {
                this.openSidePanel(e);
            }
        });
        this.bindExpandedSidePanel();
    }

    bindExpandedSidePanel() {
        var grid = $("#" + this.nameGridRelatedToSidePanel);
        $(".xico-dots").click((e) => {
            grid.find("table").width($(window).width());
            if ($(".ro-side-panel").hasClass("panel-open-expanded")) {
                $(".ro-side-panel").removeClass("panel-open-expanded").addClass("panel-open");
                grid.removeClass("expanded");
                grid.find(".k-grid-header").css("width", "60%");
                grid.find(".k-grid-content").css("width", "60%");
            }
            else {
                $(".ro-side-panel").removeClass("panel-open").addClass("panel-open-expanded");
                grid.addClass("expanded");
                grid.find(".k-grid-header").css("width", "30%");
                grid.find(".k-grid-content").css("width", "30%");
            }
        });
    }

    openSidePanel(event) {
        if (!this.sidePanelRowSelected) return false;
        var grid = $("#" + this.nameGridRelatedToSidePanel);
        grid.addClass("side-panel-openned");
        grid.find("table").width($(window).width());
        grid.find(".k-grid-header").css("width", "60%");
        grid.find(".k-grid-content").css("width", "60%");
        $("div[ro-side-panel-toggle]").removeClass("glance-view isInactive");
        $("div[ro-side-panel-toggle]").find("i").removeClass("xico-glance-view").addClass("xico-action-close");
        $(".ro-side-panel").addClass("remove-open").addClass("panel-open");
        $(".ro-side-panel").show();
        setTimeout(this.resizeSidePanelContent(), 10);
    }

    closeSidePanel(sender) {
        var isFromCloseButton = (!sender) ? false : true;
        sender = (!sender) ? $("div[ro-side-panel-toggle]") : sender;
        $(".ro-side-panel").hide();
        $(sender).addClass("glance-view");
        if (!isFromCloseButton) $(sender).addClass("isInactive");
        $(sender).find("i").removeClass("xico-action-close").addClass("xico-glance-view");

        var grid = $("#" + this.nameGridRelatedToSidePanel);
        grid.removeClass("side-panel-openned");
        grid.find("table").width("100%");
        grid.find(".k-grid-header").css("width", "100%");
        grid.find(".k-grid-content").css("width", "100%");

        if (!isFromCloseButton) grid.find(".k-grid-content").find("td.active").removeClass("active");
        this.sidePanelRowSelected = false;
    }

    // Sets the height of the content
    resizeSidePanelContent() {
        var $container = $(".ro-side-panel");
        var height = $container.outerHeight() - $container.find('.side-panel-header').outerHeight() - 15; // -15 mimics the padding of the panel
        $container.find('.side-panel-body .content').css('height', height);
    }
}