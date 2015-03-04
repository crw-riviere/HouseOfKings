$(function () {
    var game = $.connection.game,
    idleCounter,
        idleTime = 0,
    groupName = $('#group-name').val(),
    $btn = $('.action-pick-card');

    $btn.button('waiting').prop('disabled', true);

    game.client.setGameGroupInfo = function (groupInfo) {
        setGameGroupInfo(groupInfo);
    }

    game.client.addPlayer = function (player) {
        addPlayer(player)
    }

    game.client.removePlayer = function (player) {
        setAudit(player.n + ' left the group');
        removePlayer(player.i);
    }

    game.client.setGameover = function (player) {
        setGameover(player);
    }

    game.client.setNextTurn = function (player) {
        setNextTurn(player);
    }

    game.client.setTurn = function () {
        setTurn();
    }

    game.client.updateTurnInfo = function (turn) {
        updateTurnInfo(turn);
    }

    game.client.setAudit = function (message) {
        setAudit(message);
    }

    game.client.setMessage = function (message) {
        setMessage(message);
    }

    $.connection.hub.connectionSlow(function () {
        setAudit('Poverty connection to server...');
    });

    $.connection.hub.reconnected(function () {
        setAudit('Disconnected from server')
    });

    $.connection.hub.disconnected(function () {
        setAudit('Attempting to reconnect to server...');
        setTimeout(function () {
            console.log('Attempting to reconnect to server...');
            $.connection.hub.start().done(function () {
                setAudit('Connected to server');
            });
        }, 5000);
    });

    $.connection.hub.error(function (error) {
        console.log('Error: ' + error)
    });

    $.connection.hub.start().done(init);

    function init() {
        game.server.joinGroup(groupName).done(function () {
            setAudit('Joined ' + groupName);

            $(document)
                .on('click', '.action-pick-card', function () {
                    $btn.button('waiting').prop('disabled', true);
                    idleTime = 0;
                    clearInterval(idleCounter);
                    game.server.pickCard(groupName);
                })
                .on('click', '#action-replay', function () {
                    shuffleDeck();
                });
        });
    }

    function shuffleDeck() {
        setAudit('Shuffled deck');
        setMessage('');
        drawCard('', '');
        updateStats({ kc: 4, cc: 52 });
    }

    function setGameGroupInfo(groupInfo) {
        groupInfo.usrs.forEach(function (player) {
            addPlayer(player);
        });
        updateTurnInfo(groupInfo.trn);
    }

    function updateStats(turn) {
        $('#card-count').html(turn.cc);
        $('#king-count').html(turn.kc);
    }

    function addPlayer(player) {
        setAudit(player.n + ' joined the game');
        drawPlayer(player.i, player.n);
    }

    function setTurn() {
        idleCounter = setInterval(function () {
            idleTime++;
            console.log(idleTime);
            if (idleTime > 5) {
                window.location = "/";
            }
        }, 60000);
        $btn.button('pick').prop('disabled', false);
    }

    function setGameover(player) {
        $('#gameover-message').html(player.n + ' picked the last King<br/><br/><strong>Game Over</strong>');
        $('#gameover-modal').modal({ keyboard: false, backdrop: 'static' });
    }

    function updateTurnInfo(turn) {
        updateStats(turn);

        var card = turn.crd;

        if (card) {
            var number = parseCardNumber(card.n);
            var suit = parseCardSuit(card.s);
            setAudit(turn.usr.n + ' picked ' + '<span class="suit"></span>');
            setMessage('<p class="text-center">' + turn.usr.n + ' picked&emsp;<span>'+number +'&nbsp;</span><span>'+suit+'</span><br/><em class="small">card rule&nbsp;</em><strong>&emsp;' + turn.rul.t + '</strong></p>');

            drawCard(card.n, card.s);
        }

        if (turn.f) {
            setGameover(turn.usr);
        }
    }

    function setAudit(message) {
        $('#audit').html(message);
    }

    function setMessage(message) {
        $('#message').html(message);
    }

    function setNextTurn(player) {
        setAudit('Waiting for ' + player.n + ' to pick a Card');
    }
});